using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;
public class BlockParselet(Symbol terminator, Symbol seperator = null, bool wrapExpressions = false, Symbol tag = null) : IStatementParselet
{
    public Symbol Terminator { get; } = terminator;
    public Symbol Seperator { get; } = seperator;
    public Symbol Tag { get; } = tag;

    public AstNode Parse(Parser parser, Token token)
    {
        var block = new BlockNode(Seperator, Terminator);

        while (!parser.Match(Terminator))
        {
            var node = parser.ParseStatement(wrapExpressions);
            block.Children.Add(node.WithParent(block));

            if (Seperator != null && parser.IsMatch(Seperator))
            {
                parser.Consume(Seperator);
            }
        }

        return block
            .WithRange(token, parser.LookAhead(0));
    }
}
