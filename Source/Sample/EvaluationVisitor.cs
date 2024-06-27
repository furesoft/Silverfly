using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;
using Sample.Nodes;

namespace Sample;

public class EvaluationVisitor : IVisitor<Value>
{
    public Value Visit(AstNode node, Scope scope)
    {
        if (node is BlockNode block)
        {
            for (int i = 0; i < block.Children.Count - 1; i++)
            {
                Visit(block.Children[i], scope);
            }

            return Visit(block.Children.Last(), scope);
        }
        else if (node is LiteralNode literal)
        {
            return VisitLiteral(literal);
        }
        else if (node is GroupNode group)
        {
            return Visit(group.Expr);
        }
        else if (node is BinaryOperatorNode binNode)
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
        else if (node is VariableBindingNode binding)
        {
            scope.Define(binding.Name.Text.ToString(), (Value[] args) =>
            {
                var subscope = Scope.Root.NewSubScope();
                for (int i = 0; i < binding.Parameters.Count; i++)
                {
                    var index = i;
                    subscope.Define(binding.Parameters[index].Name, (Value[] argsInner) => args[index]);
                }

                return Visit(binding.Value, subscope);
            });
        }
        else if (node is NameNode name)
        {
            return scope.Get(name.Name)([]);
        }
        else if (node is CallNode call && call.FunctionExpr is NameNode func)
        {
            var args = call.Arguments.Select(Visit).Cast<Value>().ToArray();

            return scope.Get(func.Name)(args);
        }

        return new UnitValue();
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
        else if(literal.Value is UnitValue unit)
        {
            return unit;
        }

        return new UnitValue();
    }
}
