using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets;

public interface IStatementParselet
{
    AstNode Parse(Parser parser, Token token);
}
