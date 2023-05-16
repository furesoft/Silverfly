using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser;

public interface IVisitor<out T>
{
    T Visit(IAstNode node);
}
