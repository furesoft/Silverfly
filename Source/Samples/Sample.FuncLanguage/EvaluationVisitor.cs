using System.Collections.Immutable;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Text;
using Silverfly.Sample.Func.Values;
using Silverfly.Sample.Func.Nodes;
using Sivlerfly.Sample.FuncLanguage.Nodes;

namespace Silverfly.Sample.Func;

public class EvaluationVisitor : TaggedNodeVisitor<Value, Scope>
{
    public EvaluationVisitor()
    {
        For<TupleBindingNode>(Visit);
        For<EnumNode>(Visit);
        For<ImportNode>(Visit);
        For<BinaryOperatorNode>(Visit);
        For<PrefixOperatorNode>(Visit);
        For<GroupNode>(Visit);
        For<ModuleNode>(Visit);
        For<BlockNode>(Visit);
        For<VariableBindingNode>(Visit);
        For<IfNode>(Visit);
        For<LambdaNode>(Visit);
        For<NameNode>(Visit);
        For<CallNode>(Visit);
        For<LiteralNode>(Visit);
        For<TupleNode>(Visit);
    }

    Value Visit(TupleBindingNode node, Scope scope)
    {
        var value = Visit(node.Value, scope);

        for (int i = 0; i < node.Names.Count; i++)
        {
            var name = node.Names[i].Token;
            var destructedValue = value.Get(new NumberValue(i));

            scope.Define(name.Text.ToString(), destructedValue);
        }

        return UnitValue.Shared;
    }

    Value Visit(EnumNode node, Scope scope)
    {
        var memberScope = new Scope();

        for (var i = 0; i < node.Members.Count; i++)
        {
            var member = node.Members[i];
            if (member is NameNode name)
            {
                memberScope.Define(name.Token.Text.ToString(), i);
            }
        }

        memberScope.Define("__name___", node.Name);
        memberScope.Define("to_string", (Value[] args) =>
        {
            return $"enum {node.Name} = {string.Join(" | ", node.Members.Where(_ => _ is NameNode).Select(_ => ((NameNode)_).Token))}";
        });
        memberScope.Define("from_name", (Value name) =>
        {
            if (name is StringValue sv)
            {
                return memberScope.Get(sv.Value) ?? OptionValue.None;
            }

            return OptionValue.None;
        });

        scope.Define(node.Name, new ModuleValue(memberScope));

        return UnitValue.Shared;
    }

    Value Visit(ImportNode node, Scope scope)
    {
        var file = new FileInfo($"{node.Path}.f");

        var content = File.ReadAllText(file.FullName);
        var parsed = new ExpressionGrammar().Parse(content, file.FullName);
        var rewritten = parsed.Tree.Accept(new RewriterVisitor());

        if (rewritten is BlockNode block)
        {
            var module = block.Children.FirstOrDefault(_ => _ is ModuleNode);

            if (module != null)
            {
                var moduleNode = (ModuleNode)module;
                var moduleScope = scope.NewSubScope();
                rewritten.Accept(new EvaluationVisitor(), moduleScope);
                node.Range.Document.Messages.AddRange(rewritten.Range.Document.Messages);

                scope.Define(moduleNode.Name, new ModuleValue(moduleScope));

                return UnitValue.Shared;
            }
        }

        rewritten.Accept(new EvaluationVisitor(), scope);
        node.Range.Document.Messages.AddRange(rewritten.Range.Document.Messages);

        return UnitValue.Shared;
    }

    Value Visit(BinaryOperatorNode binNode, Scope scope)
    {
        var leftVisited = Visit(binNode.LeftExpr, scope);
        var rightVisited = Visit(binNode.RightExpr, scope);

        if (binNode.Operator.Text.Span == "=")
        {
            //ToDo: implement assignment
            return null;
        }

        if (binNode.Operator.Text.Span == "..")
        {
            return RangeValue.Create(leftVisited, rightVisited);
        }

        if (leftVisited is NameValue n)
        {
            binNode.LeftExpr.AddMessage(MessageSeverity.Error, $"Value '{n.Name}' not found");
            return UnitValue.Shared;
        }

        if (binNode.Operator.Text.Span == ".")
        {
            return leftVisited.Get(rightVisited);
        }

        LambdaValue func;
        if (scope.TryGet($"'{binNode.Operator.Text}", out func))
        {
            return func.Invoke(leftVisited, rightVisited);
        }
        else if (leftVisited.Members.TryGet($"'{binNode.Operator.Text}", out func))
        {
            return func.Invoke(leftVisited, rightVisited);
        }

        binNode.AddMessage(MessageSeverity.Error, $"Operator '{binNode.Operator}' is not defined");

        return UnitValue.Shared;
    }

    Value Visit(PrefixOperatorNode prefix, Scope scope)
    {
        var exprVisited = Visit(prefix.Expr, scope);

        LambdaValue func;
        if (scope.TryGet($"'{prefix.Operator.Text}", out func))
        {
            return func.Invoke(exprVisited);
        }
        else if (exprVisited.Members.TryGet($"'{prefix.Operator.Text}", out func))
        {
            return func.Invoke(exprVisited);
        }

        prefix.AddMessage(MessageSeverity.Error, $"Operator '{prefix.Operator}' is not defined");

        return UnitValue.Shared;
    }

    Value Visit(GroupNode group, Scope scope) => Visit(group.Expr, scope);
    Value Visit(ModuleNode module, Scope scope) => UnitValue.Shared;

    Value Visit(BlockNode block, Scope scope)
    {
        for (int i = 0; i < block.Children.Count - 1; i++)
        {
            Visit(block.Children[i], scope);
        }

        return Visit(block.Children.Last(), scope); // the last expression is the return value
    }

    //@enter(on_enter)
    //@leave(on_leave)
    private void CallAnnotationRef(string annotationName, AnnotatedNode node, Scope scope, params Value[] args)
    {
        foreach (var annotation in node.Annotations)
        {
            if (annotation.FunctionExpr is NameNode n && n.Token.Text.Span == annotationName)
            {
                var funcRef = annotation.Arguments[0];
                var evaluatedFuncRef = (LambdaValue)Visit(funcRef, scope);
                evaluatedFuncRef.Value.Invoke(args);

                return;
            }
        }
    }

    Value Visit(VariableBindingNode binding, Scope scope)
    {
        if (binding.Parameters.Count == 0)
        {
            var definition = Visit(binding.Value, scope);
            AddAnnotations(binding, definition, scope);

            scope.Define(binding.Name.Text.ToString(), definition);
        }
        else
        {
            Func<Value[],Value> func = args =>
            {
                CallAnnotationRef("enter", binding, scope, new StringValue(binding.Name.Text.ToString()));
                var res = CallFunction(binding.Parameters, args, binding.Value);

                CallAnnotationRef("exit", binding, scope, new StringValue(binding.Name.Text.ToString()), res);

                return res;
            };

            var value = new LambdaValue(func, null);
            AddAnnotations(binding, value, scope);

            scope.Define(binding.Name.Text.ToString(), value);
        }

        return UnitValue.Shared;
    }

    Value Visit(IfNode ifNode, Scope scope)
    {
        var evaluatedCondition = Visit(ifNode.Condition, scope);

        if (evaluatedCondition.IsTruthy())
        {
            return Visit(ifNode.TruePart, scope.NewSubScope());
        }
        else
        {
            return Visit(ifNode.FalsePart, scope.NewSubScope());
        }
    }

    Value Visit(LambdaNode lambda, Scope scope)
    {
        return new LambdaValue(args => CallFunction(lambda.Parameters, args, lambda.Value), lambda);
    }

    Value Visit(NameNode name, Scope scope)
    {
        return scope.Get(name.Token.Text.ToString())! ?? new NameValue(name.Token.Text.ToString());
    }

    Value Visit(CallNode call, Scope scope)
    {
        return call.FunctionExpr switch
        {
            NameNode func => VisitNamedFunction(func, call, scope),
            LambdaNode funcGroup => VisitLambdaFunction(funcGroup, call, scope),
            CallNode c => Visit(c, scope),
            _ => VisitOtherFunction(call, scope)
        };
    }

    void AddAnnotations(AstNode node, Value value, Scope scope)
    {
        if (node is AnnotatedNode an)
        {
            foreach (var anotationNode in an.Annotations)
            {
                var annotation = new Annotation
                (
                    ((NameNode)anotationNode.FunctionExpr).Token.Text.ToString(),
                    anotationNode.Arguments.Select(n => Visit(n, scope)).ToList()
                );

                value.Annotations.Add(annotation);
            }
        }
    }

    Value VisitOtherFunction(CallNode call, Scope scope)
    {
        var args = call.Arguments.Select(_ => Visit(_, scope)).ToArray();
        var func = Visit(call.FunctionExpr, scope);

        if (func is LambdaValue l)
        {
            return (Value)l.Value.DynamicInvoke([args]);
        }

        return UnitValue.Shared;
    }

    private Value VisitLambdaFunction(LambdaNode funcGroup, CallNode call, Scope scope)
    {
        var args = call.Arguments.Select(_ => Visit(_, scope)).ToArray();
        var f = Visit(funcGroup, scope);

        if (f is LambdaValue l)
        {
            return l.Value(args);
        }

        return UnitValue.Shared;
    }

    private Value VisitNamedFunction(NameNode func, CallNode call, Scope scope)
    {
        var args = call.Arguments.Select(arg => Visit(arg, scope)).ToArray();
        var f = scope.Get(func.Token.Text.ToString());

        if (f is LambdaValue lambda)
        {
            return lambda.Value(args);
        }

        func.AddMessage(MessageSeverity.Error, $"Function '{func.Token}' not found");

        return UnitValue.Shared;
    }

    protected override Value VisitUnknown(AstNode node, Scope tag)
    {
        if (node is InvalidNode invalid)
        {
            node.Range.Document.Messages.Add(Message.Error("Cannot evaluate " + invalid.Token.Text, invalid.Range));
        }

        return UnitValue.Shared;
    }

    private Value CallFunction(ImmutableList<NameNode> parameters, Value[] args, AstNode definition)
    {
        var subScope = Scope.Root.NewSubScope();
        for (int i = 0; i < parameters.Count; i++)
        {
            var index = i;
            subScope.Define(parameters[index].Token.Text.ToString(), args[index]);
        }

        return Visit(definition, subScope);
    }

    private Value Visit(LiteralNode literal, Scope scope)
    {
        return literal.Value switch
        {
            double d => new NumberValue(d),
            ulong ul => new NumberValue(ul),
            bool b => new BoolValue(b),
            string s => new StringValue(s),
            UnitValue unit => unit,
            ImmutableList<AstNode> v => new ListValue(v.Select(_ => Visit(_, scope)).ToList()),
            _ => UnitValue.Shared
        };
    }

    Value Visit(TupleNode tuple, Scope scope)
    {
        return new TupleValue(tuple.Values.Select(_ => Visit(_, scope)).ToList());
    }
}
