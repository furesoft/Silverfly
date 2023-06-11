namespace Furesoft.PrattParser;

public interface IVisitor<in TNode, out TReturn>
{
    TReturn Visit(TNode node);
}

public interface IVisitor<in TNode>
{
    void Visit(TNode node);
}
