namespace Furesoft.PrattParser.Expressions;

public interface IAstNode {
    //public SourceRange Range { get; set; }
    public T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
