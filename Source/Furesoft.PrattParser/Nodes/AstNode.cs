using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Nodes;

public abstract record AstNode
{
    public SourceRange Range { get; set; }
    public AstNode? Parent { get; set; }

    public AstNode WithRange(Token token) => this with { Range = new SourceRange(token.Document, token.GetSourceSpanStart(), token.GetSourceSpanEnd()) };
    public AstNode WithRange(SourceRange range) => this with { Range = range };
    public AstNode WithRange(SourceDocument document, SourceSpan start, SourceSpan end) => this with { Range = new SourceRange(document, start, end) };
    public AstNode WithRange(Token start, Token end) => this with { Range = new SourceRange(start.Document, start.GetSourceSpanStart(), end.GetSourceSpanEnd()) };
    public AstNode WithParent(AstNode parent) => this with { Parent = parent };

    public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    public void Accept(IVisitor visitor) => visitor.Visit(this);
}