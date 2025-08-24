using MrKWatkins.Ast.Listening;
using MrKWatkins.Ast.Processing;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Sample.Func.Nodes;
using Silverfly.Sample.Func.Values;
using Silverfly.Text;
using Sivlerfly.Sample.FuncLanguage.Nodes;

namespace Silverfly.Sample.Func;

public class EvaluationListener
{
    public static CompositeListener<EvaluationContext, AstNode> Listener = CompositeListener<EvaluationContext, AstNode>
        .Build()
        .With(new BinaryListener())
        .With(new PrefixListener())
        .With(new GroupListener())
        .With(new BlockListener())
        .With(new IfListener())
        .With(new TupleListener())
        .With(new ImportListener())
        .With(new EnumListener())
        .With(new NameListener())
        .With(new LiteralListener())
        .With(new TupleBindingListener())
        .With(new VariableBindingListener())
        .With(new LambdaListener())
        .With(new CallListener())
        .ToListener();

    private class BinaryListener : Listener<EvaluationContext, AstNode, BinaryOperatorNode>
    {
        protected override void ListenToNode(EvaluationContext context, BinaryOperatorNode node)
        {
            Listen(context, node.Left);
            Listen(context, node.Right);

            var leftVisited = context.Stack.Pop();
            var rightVisited = context.Stack.Pop();

            if (node.Operator.Text.Span is "=")
            {
                //ToDo: implement assignment
                return;
            }

            if (node.Operator.Text.Span is "..")
            {
                context.Stack.Push(RangeValue.Create(leftVisited, rightVisited));
            }

            if (leftVisited is NameValue n)
            {
                node.Left.AddMessage(MessageSeverity.Error, $"Value '{n.Name}' not found");
                return;
            }

            if (node.Operator.Text.Span is ".")
            {
                context.Stack.Push(leftVisited.Get(rightVisited));
                return;
            }

            LambdaValue func;
            if (context.Scope.TryGet($"'{node.Operator.Text}", out func))
            {
                context.Stack.Push(func.Invoke(leftVisited, rightVisited));
                return;
            }

            if (leftVisited.Members.TryGet($"'{node.Operator.Text}", out func))
            {
                context.Stack.Push(func.Invoke(leftVisited, rightVisited));
                return;
            }

            node.AddMessage(MessageSeverity.Error, $"Operator '{node.Operator}' is not defined");
        }
    }

    private class PrefixListener : Listener<EvaluationContext, AstNode, PrefixOperatorNode>
    {
        protected override void ListenToNode(EvaluationContext context, PrefixOperatorNode node)
        {
            Listen(context, node.Expr);
            var exprVisited = context.Stack.Pop();

            LambdaValue func;
            if (context.Scope.TryGet($"'{node.Operator.Text}", out func))
            {
                context.Stack.Push(func.Invoke(exprVisited));
                return;
            }

            if (exprVisited.Members.TryGet($"'{node.Operator.Text}", out func))
            {
                context.Stack.Push(func.Invoke(exprVisited));
            }

            node.AddMessage(MessageSeverity.Error, $"Operator '{node.Operator}' is not defined");
        }
    }

    private class GroupListener : Listener<EvaluationContext, AstNode, GroupNode>
    {
        protected override void ListenToNode(EvaluationContext context, GroupNode node)
        {
            Listen(context, node.Expr);
        }
    }

    private class BlockListener : Listener<EvaluationContext, AstNode, BlockNode>
    {
        protected override void ListenToNode(EvaluationContext context, BlockNode node)
        {
            foreach (var child in node.Children)
            {
                Listen(context, child);
            }
        }
    }

    private class IfListener : Listener<EvaluationContext, AstNode, IfNode>
    {
        protected override void ListenToNode(EvaluationContext context, IfNode node)
        {
            Listen(context, node.Condition);
            var evaluatedCondition = context.Stack.Pop();

            if (evaluatedCondition.IsTruthy())
            {
                Listen(context.NewSubScope(), node.TruePart);
            }
            else
            {
                Listen(context.NewSubScope(), node.FalsePart);
            }
        }
    }

    private class TupleListener : Listener<EvaluationContext, AstNode, TupleNode>
    {
        protected override void ListenToNode(EvaluationContext context, TupleNode node)
        {
            var values = new List<Value>();
            foreach (var child in node.Children)
            {
                Listen(context, child);
                values.Add(context.Stack.Pop());
            }

            context.Stack.Push(new TupleValue(values));
        }
    }

    private class ImportListener : Listener<EvaluationContext, AstNode, ImportNode>
    {
        protected override void ListenToNode(EvaluationContext context, ImportNode node)
        {
            var file = new FileInfo($"{node.Path}.f");

            var content = File.ReadAllText(file.FullName);
            var parsed = new ExpressionGrammar().Parse(content, file.FullName);
            var pipeline = Pipeline<AstNode>.Build(_ => _.AddStage<LiteralReplacer>());
            pipeline.Run(parsed.Tree);

            if (parsed.Tree is BlockNode block)
            {
                var module = block.Children.FirstOrDefault(_ => _ is ModuleNode);

                if (module != null)
                {
                    var moduleNode = (ModuleNode)module;
                    var moduleScope = context.Scope.NewSubScope();
                    Listener.Listen(context, parsed.Tree);
                    node.Range.Document.Messages.AddRange(parsed.Tree.Range.Document.Messages);

                    context.Scope.Define(moduleNode.Name, new ModuleValue(moduleScope));
                    return;
                }
            }

            Listener.Listen(context, parsed.Tree);
            node.Range.Document.Messages.AddRange(parsed.Tree.Range.Document.Messages);
        }
    }

    private class EnumListener : Listener<EvaluationContext, AstNode, EnumNode>
    {
        protected override void ListenToNode(EvaluationContext context, EnumNode node)
        {
            var memberScope = new Scope();

            for (var i = 0; i < node.Children.Count; i++)
            {
                var member = node.Children[i];
                if (member is NameNode name)
                {
                    memberScope.Define(name.Token.Text.ToString(), i);
                }
            }

            memberScope.Define("__name___", node.Name);
            memberScope.Define("to_string", (Value[] args) =>
            {
                return
                    $"enum {node.Name} = {string.Join(" | ", node.Children.Where(_ => _ is NameNode).Select(_ => ((NameNode)_).Token))}";
            });
            memberScope.Define("from_name", (Value name) =>
            {
                if (name is StringValue sv)
                {
                    return memberScope.Get(sv.Value) ?? OptionValue.None;
                }

                return OptionValue.None;
            });

            context.Scope.Define(node.Name, new ModuleValue(memberScope));
        }
    }

    private class NameListener : Listener<EvaluationContext, AstNode, NameNode>
    {
        protected override void ListenToNode(EvaluationContext context, NameNode node)
        {
            var value = context.Scope.Get(node.Token.Text.ToString())! ?? new NameValue(node.Token.Text.ToString());
            context.Stack.Push(value);
        }
    }

    private class LiteralListener : Listener<EvaluationContext, AstNode, LiteralNode>
    {
        protected override void ListenToNode(EvaluationContext context, LiteralNode node)
        {
            var value = UnitValue.Shared;

            if (node.Value is double d)
            {
                value = new NumberValue(d);
            }
            else if (node.Value is bool b)
            {
                value = new BoolValue(b);
            }
            else if (node.Value is string s)
            {
                value = new StringValue(s);
            }
            else if (node.Value is UnitValue unit)
            {
                value = unit;
            }
            else if (node.Value is IEnumerable<AstNode> v)
            {
                var values = new List<Value>();

                foreach (var val in v)
                {
                    Listen(context, val);
                    values.Add(context.Stack.Pop());
                }

                value = values;
            }

            context.Stack.Push(value);
        }
    }

    private class TupleBindingListener : Listener<EvaluationContext, AstNode, TupleBindingNode>
    {
        protected override void ListenToNode(EvaluationContext context, TupleBindingNode node)
        {
            Listen(context, node.Value);
            var value = context.Stack.Pop();

            for (var i = 0; i < node.Names.Count(); i++)
            {
                var name = node.Names[i].Token;
                var destructedValue = value.Get(new NumberValue(i));

                context.Scope.Define(name.Text.ToString(), destructedValue);
            }
        }
    }

    private class VariableBindingListener : CallListenerBase<VariableBindingNode>
    {
        protected override void ListenToNode(EvaluationContext context, VariableBindingNode node)
        {
            if (!node.Parameters.Any())
            {
                Listen(context, node.Value);
                var definition = context.Stack.Pop();
                AddAnnotations(node, definition, context);

                context.Scope.Define(node.Name.Text.ToString(), definition);
            }
            else
            {
                Func<Value[], Value> func = args =>
                {
                    CallAnnotationRef("enter", node, context, new StringValue(node.Name.Text.ToString()));
                    var res = CallFunction(node.Parameters.OfType<NameNode>(), args, node.Value, context);

                    CallAnnotationRef("exit", node, context, new StringValue(node.Name.Text.ToString()), res);

                    return res;
                };

                var value = new LambdaValue(func, null);
                AddAnnotations(node, value, context);

                context.Scope.Define(node.Name.Text.ToString(), value);
            }
        }

        //@enter(on_enter)
        //@leave(on_leave)
        private void CallAnnotationRef(string annotationName, AnnotatedNode node, EvaluationContext context,
            params Value[] args)
        {
            foreach (var annotation in node.Annotations)
            {
                if (annotation.FunctionExpr is NameNode n && n.Token.Text.Span == annotationName)
                {
                    var funcRef = annotation.Arguments[0];
                    Listen(context, funcRef);
                    var evaluatedFuncRef = (LambdaValue)context.Stack.Pop();
                    evaluatedFuncRef.Value.Invoke(args);

                    return;
                }
            }
        }
    }

    private class LambdaListener : CallListenerBase<LambdaNode>
    {
        protected override void ListenToNode(EvaluationContext context, LambdaNode node)
        {
            var value = new LambdaValue(args => CallFunction(node.Parameters, args, node.Value, context), node);
            context.Stack.Push(value);
        }
    }

    private class CallListener : Listener<EvaluationContext, AstNode, CallNode>
    {
        protected override void ListenToNode(EvaluationContext context, CallNode node)
        {
            switch (node.FunctionExpr)
            {
                case NameNode func:
                    VisitNamedFunction(func, node, context);
                    break;
                case LambdaNode funcGroup:
                    VisitLambdaFunction(funcGroup, node, context);
                    break;
                case CallNode func:
                    Listen(context, func);
                    break;
                default:
                    VisitOtherFunction(node, context);
                    break;
            }
        }

        private void VisitOtherFunction(CallNode call, EvaluationContext context)
        {
            var args = new List<Value>();
            foreach (var arg in call.Arguments)
            {
                Listen(context, arg);
                args.Add(context.Stack.Pop());
            }

            Listen(context, call.FunctionExpr);
            var func = context.Stack.Pop();

            if (func is LambdaValue l)
            {
                var result = (Value)l.Value.DynamicInvoke(args);
                context.Stack.Push(result);
            }
        }

        private void VisitNamedFunction(NameNode func, CallNode call, EvaluationContext context)
        {
            var args = new List<Value>();
            foreach (var arg in call.Arguments)
            {
                Listen(context, arg);
                args.Add(context.Stack.Pop());
            }

            var f = context.Scope.Get(func.Token.Text.ToString());

            if (f is LambdaValue lambda)
            {
                context.Stack.Push(lambda.Value([.. args]));
            }

            func.AddMessage(MessageSeverity.Error, $"Function '{func.Token}' not found");
        }

        private void VisitLambdaFunction(LambdaNode funcGroup, CallNode call, EvaluationContext context)
        {
            var args = new List<Value>();
            foreach (var arg in call.Arguments)
            {
                Listen(context, arg);
                args.Add(context.Stack.Pop());
            }

            Listen(context, funcGroup);
            var f = context.Stack.Pop();

            if (f is LambdaValue l)
            {
                context.Stack.Push(l.Value([.. args]));
            }
        }
    }

    private class AnnotatableListener<T> : Listener<EvaluationContext, AstNode, T>
        where T : AstNode
    {
        protected void AddAnnotations(AstNode node, Value value, EvaluationContext context)
        {
            if (node is AnnotatedNode an)
            {
                foreach (var anotationNode in an.Annotations)
                {
                    var arguments = new List<Value>();
                    foreach (var arg in anotationNode.Arguments)
                    {
                        Listen(context, arg);
                        arguments.Add(context.Stack.Pop());
                    }

                    var annotation = new Annotation
                    (
                        ((NameNode)anotationNode.FunctionExpr).Token.Text.ToString(),
                        arguments
                    );

                    value.Annotations.Add(annotation);
                }
            }
        }
    }

    private class CallListenerBase<T> : AnnotatableListener<T>
        where T : AstNode
    {
        protected Value CallFunction(IEnumerable<NameNode> parameters, Value[] args, AstNode definition,
            EvaluationContext context)
        {
            var subScope = Scope.Root.NewSubScope();
            for (var i = 0; i < parameters.Count(); i++)
            {
                var index = i;
                subScope.Define(parameters.Skip(index).First().Token.Text.ToString(), args[index]);
            }

            Listen(context, definition);

            return context.Stack.Pop();
        }
    }
}
