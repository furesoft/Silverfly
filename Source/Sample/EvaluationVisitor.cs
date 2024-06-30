using System.Collections.Immutable;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Sample.Nodes;

namespace Sample;

public class EvaluationVisitor : IVisitor<Value>
{
    public Value Visit(AstNode node, Scope scope)
    {
        switch (node)
        {
            case BlockNode block:
                {
                    for (int i = 0; i < block.Children.Count - 1; i++)
                    {
                        Visit(block.Children[i], scope);
                    }

                    return Visit(block.Children.Last(), scope); // the last expression is the return value
                }
            case LiteralNode literal:
                return VisitLiteral(literal);
            case GroupNode group:
                return Visit(group.Expr);
            case BinaryOperatorNode binNode:
                {
                    var leftVisited = Visit(binNode.LeftExpr, scope);
                    var rightVisited = Visit(binNode.RightExpr, scope);

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
            case VariableBindingNode binding:
                scope.Define(binding.Name.Text.ToString(), args => CallFunction(binding.Parameters, args, binding.Value));

                return UnitValue.Shared;
            case LambdaNode lambda:
                return new LambdaValue(args => CallFunction(lambda.Parameters, args, lambda.Value));
            case NameNode name:
                return scope.Get(name.Name)!([]);
            case CallNode { FunctionExpr: NameNode func } call:
                {
                    var args = call.Arguments.Select(Visit).ToArray();

                    return scope.Get(func.Name)!(args);
                }
            case CallNode { FunctionExpr: LambdaNode funcGroup } gcall:
                {
                    var args = gcall.Arguments.Select(Visit).ToArray();
                    var f = Visit(funcGroup);

                    if (f is LambdaValue l)
                    {
                        return l.Value(args);
                    }

                    break;
                }
        }

        Console.WriteLine("cannot handle: " + node);
        return UnitValue.Shared;
    }

    private Value CallFunction(ImmutableList<NameNode> parameters, Value[] args, AstNode definition)
    {
        var subScope = Scope.Root.NewSubScope();
        for (int i = 0; i < parameters.Count; i++)
        {
            var index = i;
            subScope.Define(parameters[index].Name, (Value[] _) => args[index]);
        }

        return Visit(definition, subScope);
    }

    public Value Visit(AstNode node)
    {
        return Visit(node, Scope.Root);
    }

    private Value VisitLiteral(LiteralNode literal)
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
            return new ListValue(v.Select(Visit).ToList());
        }

        return UnitValue.Shared;
    }
}
