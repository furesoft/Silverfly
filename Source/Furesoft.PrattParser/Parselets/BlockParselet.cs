using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

public class BlockParselet(Symbol seperator, Symbol terminator, int bindingPower) : IInfixParselet
{
    public AstNode Parse(Parser parser, AstNode left, Token token)
    {
        var node = new BlockNode { SeperatorSymbol = seperator, Children = parser.ParseSeperated(seperator, terminator) };
        node.Children.Insert(0, left);

        return node.WithRange(left.Range.Document, left.Range.Start, parser.LookAhead(0).GetSourceSpanEnd());
    }

    public int GetBindingPower()
    {
        return bindingPower;
    }
}
