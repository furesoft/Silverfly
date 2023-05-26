using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Text;

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
            token.Document.Messages.Add(Message.Error("The left-hand side of an assignment must be a name."));
        }

        var name = ((NameAstNode)left).Name;

        return new AssignNode(name, token.Text, right).WithRange(left.Range.Document, left.Range.Start, right.Range.End);
    }

    public int GetBindingPower()
    {
        return (int)BindingPower.Assignment;
    }
}
