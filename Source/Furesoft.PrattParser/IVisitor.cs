using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser;

public interface IVisitor<out T>
{
    T Visit(AstNode node);
}
