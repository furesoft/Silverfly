using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A binary arithmetic expression like "a + b" or "c ^ d".
/// </summary>
public class BinaryOperatorAstNode : IAstNode
{
    public IAstNode LeftExpr { get; }
    public Symbol Operator { get; }
    public IAstNode RightExpr { get; }

    public BinaryOperatorAstNode(IAstNode leftExpr, Symbol op, IAstNode rightExpr)
    {
        LeftExpr = leftExpr;
        Operator = op;
        RightExpr = rightExpr;
    }

    public void Print(StringBuilder sb)
    {
    }
}
