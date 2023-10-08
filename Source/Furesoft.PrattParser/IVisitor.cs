using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser;

public interface IVisitor<out TReturn>
{
    TReturn Visit(AstNode node);
}

public interface IVisitor
{
    void Visit(AstNode node);
}
