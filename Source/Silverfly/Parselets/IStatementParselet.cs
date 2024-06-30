using Silverfly.Nodes;

namespace Silverfly.Parselets;

public interface IStatementParselet
{
    AstNode Parse(Parser parser, Token token);
}
