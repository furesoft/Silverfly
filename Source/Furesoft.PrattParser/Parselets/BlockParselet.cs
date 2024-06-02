using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;
public class BlockParselet(Symbol terminator, Symbol seperator = null, bool wrapExpressions = false) : IStatementParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var statements = new List<AstNode>();

        while (!parser.Match(terminator))
        {
            statements.Add(parser.ParseStatement(wrapExpressions));

            if (seperator != null && parser.IsMatch(seperator))
            {
                parser.Consume(seperator);
            }
        }

        return new BlockNode(seperator, statements)
            .WithRange(token, parser.LookAhead(0));
    }
}