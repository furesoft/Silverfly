using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Nodes.Operators;

namespace Sample;

public class EvaluationVisitor : IVisitor<double>
{
    public double Visit(AstNode node)
    {
        if (node is BlockNode block)
        {
            return Visit(block.Children.Last());
        }
        else if (node is LiteralNode<double> literal)
        {
            return literal.Value;
        }
        else if (node is BinaryOperatorNode binNode)
        {
            var leftVisited = Visit(binNode.LeftExpr);
            var rightVisited = Visit(binNode.RightExpr);

            return binNode.Operator.Name switch
            {
                "+" => leftVisited + rightVisited,
                "*" => leftVisited * rightVisited,
                _ => 0
            };
        }

        return 0;
    }
}
