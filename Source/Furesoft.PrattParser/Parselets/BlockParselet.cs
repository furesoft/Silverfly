using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

public class BlockParselet : IInfixParselet<AstNode>
{
    private readonly Symbol _seperator;
    private readonly Symbol _terminator;
    private readonly int _bindingPower;

    public BlockParselet(Symbol seperator, Symbol terminator, int bindingPower)
    {
        _seperator = seperator;
        _terminator = terminator;
        _bindingPower = bindingPower;
    }

    public AstNode Parse(Parser<AstNode> parser, AstNode left, Token token)
    {
        var node = new BlockNode();
        node.SeperatorSymbol = _seperator;
        node.Children = parser.ParseSeperated(_seperator, _terminator);
        node.Children.Insert(0, left);

        return node.WithRange(left.Range.Document, left.Range.Start, parser.LookAhead(0).GetSourceSpanEnd());
    }

    public int GetBindingPower()
    {
        return _bindingPower;
    }
}
