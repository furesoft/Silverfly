namespace Furesoft.PrattParser;

public interface IVisitor<TNode, TReturn>
{
    TReturn Visit(TNode node);
}

public interface IVisitor<TNode>
{
    void Visit(TNode node);
}
