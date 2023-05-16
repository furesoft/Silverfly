namespace Furesoft.PrattParser.Nodes;

public class AstNode
{
    public SourceRange Range { get; set; }
    
    public T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
