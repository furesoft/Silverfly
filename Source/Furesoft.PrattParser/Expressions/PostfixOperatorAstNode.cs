using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A postfix unary arithmetic expression like "a!"
/// </summary>
public class PostfixOperatorAstNode : IAstNode
{
    public IAstNode Expr { get; }
    public Symbol Operator { get; }

    public PostfixOperatorAstNode(IAstNode left, Symbol op)
    {
        Expr = left;
        Operator = op;
    }
}
