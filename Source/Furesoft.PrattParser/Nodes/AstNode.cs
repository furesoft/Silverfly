namespace Furesoft.PrattParser.Nodes;

public abstract class AstNode
{
    public SourceRange Range { get; set; }

    public AstNode WithRange(Token token)
    {
        Range = new(token.Document, token.GetSourceSpanStart(), token.GetSourceSpanEnd());
        
        return this;
    }
    
    public AstNode WithRange(SourceDocument document, SourceSpan start, SourceSpan end)
    {
        Range = new(document, start, end);
        
        return this;
    }
    
    public AstNode WithRange(Token start, Token end)
    {
        Range = new(start.Document, start.GetSourceSpanStart(), end.GetSourceSpanEnd());
        
        return this;
    }

    public T Accept<T>(IVisitor<AstNode, T> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept(IVisitor<AstNode> visitor)
    {
        visitor.Visit(this);
    }
}
