using Silverfly.Nodes;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Silverfly.Parselets;
public class BlockParselet(Symbol terminator, Symbol separator = null, bool wrapExpressions = false, Symbol tag = null) : IStatementParselet, ISymbolDiscovery
{
    public Symbol Terminator { get; } = terminator;
    public Symbol Separator { get; } = separator;
    public Symbol Tag { get; } = tag;

    public AstNode Parse(Parser parser, Token token)
    {
        var block = new BlockNode(Separator, Terminator);
        var children = new List<AstNode>();

        while (!parser.Match(Terminator) && !parser.IsAtEnd())
        {
            var node = parser.ParseStatement(wrapExpressions);

            if (node is not InvalidNode)
            {
                children.Add(node with { Parent = block });
            }

            if (Separator != null && parser.IsMatch(Separator))
            {
                parser.Consume(Separator);
            }
        }

        return block
            .WithChildren(children.ToImmutableList())
            .WithRange(token, parser.LookAhead(0));
    }

    public Symbol[] GetSymbols()
    {
        return [terminator, separator];
    }
}
