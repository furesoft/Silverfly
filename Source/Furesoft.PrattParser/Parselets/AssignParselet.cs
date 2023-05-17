using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parses assignment expressions like "a = b". The left side of an assignment
/// expression must be a simple name like "a", and expressions are
/// right-associative. (In other words, "a = b = c" is parsed as "a = (b = c)").
/// </summary>
public class AssignParselet : IInfixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, AstNode left, Token token)
    {
        var right = parser.Parse((int)BindingPower.Assignment - 1);

        if (!(left is NameAstNode))
        {
            throw new ParseException("The left-hand side of an assignment must be a name.");
        }

        var name = ((NameAstNode)left).Name;

        return new AssignAstNode(name, right).WithRange(left.Range.Start, right.Range.End);
    }

    public int GetBindingPower()
    {
        return (int)BindingPower.Assignment;
    }
}
