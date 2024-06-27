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
            for (int i = 0; i < block.Children.Count - 1, i++)
            {
                Visit(block.Children[i], scope);
            }

            return Visit(block.Children.Last(), scope);
        }
        else if (node is LiteralNode literal)
        {
            return new NumberValue(Convert.ToDouble(literal.Value));
        }
        else if (node is GroupNode group)
        {
            if (group.Expr is null)
            {
                return new UnitValue();
            }

            return Visit(group.Expr);
        }
        else if (node is BinaryOperatorNode binNode)
        {
            var leftVisited = Visit(binNode.LeftExpr, scope);
            var rightVisited = Visit(binNode.RightExpr, scope);

            return binNode.Operator.Name switch
            {
                "+" => leftVisited + rightVisited,
                "-" => leftVisited - rightVisited,
                "*" => leftVisited * rightVisited,
                "/" => leftVisited / rightVisited,
                _ => 0
            };
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
            var args = call.Arguments.Select(Visit).Cast<object>().ToArray();

            return scope.Get(func.Name)(args);
        }

        return 0;
    }

    public double Visit(AstNode node)
    {
        return Visit(node, Scope.Root);
    }
}
