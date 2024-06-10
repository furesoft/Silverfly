using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;
public class BlockParselet(Symbol terminator, Symbol seperator = null, bool wrapExpressions = false) : IStatementParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var block = new BlockNode(seperator, terminator);

        while (!parser.Match(terminator))
        {
            var node = parser.ParseStatement(wrapExpressions);
            block.Children.Add(node.WithParent(block));

            if (seperator != null && parser.IsMatch(seperator))
            {
                parser.Consume(seperator);
            }
        }

        return block
            .WithRange(token, parser.LookAhead(0));
    }
}