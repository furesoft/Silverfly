using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Nodes;

public abstract class AstNode
{
    public SourceRange Range { get; private set; }
    public AstNode Parent { get; private set; }

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

    public AstNode WithParent(AstNode parent)
    {
        Parent = parent;

        return this;
    }

    public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);

    public void Accept(IVisitor visitor) => visitor.Visit(this);
}
