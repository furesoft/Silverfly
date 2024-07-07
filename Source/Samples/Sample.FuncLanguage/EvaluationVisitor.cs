using System.Collections.Immutable;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Sample.FuncLanguage.Nodes;

namespace Sample.FuncLanguage;

public class EvaluationVisitor : TaggedNodeVisitor<Value, Scope>
{
    public EvaluationVisitor()
    {
        For<LiteralNode>(Visit);
        For<BlockNode>(Visit);
        For<GroupNode>(Visit);
        For<BinaryOperatorNode>(Visit);
        For<VariableBindingNode>(Visit);
        For<IfNode>(Visit);
        For<LambdaNode>(Visit);
        For<NameNode>(Visit);
        For<CallNode>(Visit);
        For<TupleNode>(Visit);
    }

    Value Visit(BinaryOperatorNode binNode, Scope scope)
    {
        var leftVisited = Visit(binNode.LeftExpr, scope);
        var rightVisited = Visit(binNode.RightExpr, scope);

        if (binNode.Operator == (Symbol)".")
        {
            if (leftVisited is IObject o)
            {
                return o.Get(rightVisited);
            }
        }

        var leftValue = ((NumberValue)leftVisited).Value;
        var rightValue = ((NumberValue)rightVisited).Value;

        var result = binNode.Operator.Name switch
        {
            "+" => leftValue + rightValue,
            "-" => leftValue - rightValue,
            "*" => leftValue * rightValue,
            "/" => leftValue / rightValue,
            _ => 0
        };

        return new NumberValue(result);
    }

    Value Visit(GroupNode group, Scope scope) => Visit(group.Expr, scope);

    Value Visit(BlockNode block, Scope scope)
    {
        for (int i = 0; i < block.Children.Count - 1; i++)
        {
            Visit(block.Children[i], scope);
        }

        return Visit(block.Children.Last(), scope); // the last expression is the return value
    }

    Value Visit(VariableBindingNode binding, Scope scope)
    {
        if (binding.Parameters.Count == 0)
        {
            scope.Define(binding.Name.Text.ToString(), Visit(binding.Value, scope));
        }
        else
        {
            scope.Define(binding.Name.Text.ToString(), args => CallFunction(binding.Parameters, args, binding.Value));
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
        return scope.Get(name.Name)!;
    }

    Value Visit(CallNode call, Scope scope)
    {
        return call.FunctionExpr switch
        {
            NameNode func => VisitNamedFunction(func, call, scope),
            LambdaNode funcGroup => VisitLambdaFunction(funcGroup, call, scope),
            CallNode c => Visit(c, scope),
            _ => UnitValue.Shared
        };
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
        var f = (LambdaValue?)scope.Get(func.Name);

        return f!.Value!(args);
    }

    protected override Value VisitUnknown(AstNode node, Scope tag)
    {
        Console.WriteLine("cannot handle: " + node);

        return UnitValue.Shared;
    }

    private Value CallFunction(ImmutableList<NameNode> parameters, Value[] args, AstNode definition)
    {
        var subScope = Scope.Root.NewSubScope();
        for (int i = 0; i < parameters.Count; i++)
        {
            var index = i;
            subScope.Define(parameters[index].Name, args[index]);
        }

        return Visit(definition, subScope);
    }

    private Value Visit(LiteralNode literal, Scope scope)
    {
        if (literal.Value is double d)
        {
            return new NumberValue(d);
        }
        else if (literal.Value is bool b)
        {
            return new BoolValue(b);
        }
        else if (literal.Value is UnitValue unit)
        {
            return unit;
        }
        else if (literal.Value is ImmutableList<AstNode> v)
        {
            return new ListValue(v.Select(_ => Visit(_, scope)).ToList());
        }

        return UnitValue.Shared;
    }

    Value Visit(TupleNode tuple, Scope scope)
    {
        return new TupleValue(tuple.Values.Select(_ => Visit(_, scope)).ToList());
    }
}
