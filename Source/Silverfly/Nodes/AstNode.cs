using Silverfly.Text;

namespace Silverfly.Nodes;

#nullable enable

/// <summary>
/// Abstract base class for nodes in an abstract syntax tree (AST).
/// </summary>
public abstract class AstNode : MrKWatkins.Ast.PropertyNode<AstNode>
{
    /// <summary>
    /// Gets the source range associated with this AST node.
    /// </summary>
    public SourceRange Range { get; set; }

    /// <summary>
    /// A property to store extra information
    /// </summary>
    public object? Tag
    {
        get => Properties.GetOrDefault<object>(nameof(Tag));
        set => Properties.Set(nameof(Tag), value);
    }

    /// <summary>
    /// Sets the source range of the AST node using the given token.
    /// </summary>
    /// <param name="token">The token providing the source range.</param>
    /// <returns>A new instance of the AST node with the updated range.</returns>
    public AstNode WithRange(Token token)
    {
        Range = new SourceRange(token.Document, token.GetSourceSpanStart(), token.GetSourceSpanEnd());

        return this;
    }

    /// <summary>
    /// Sets the source range of the AST node.
    /// </summary>
    /// <param name="range">The source range to set.</param>
    /// <returns>A new instance of the AST node with the updated range.</returns>
    public AstNode WithRange(SourceRange range)
    {
        Range = range;
        return this;
    }

    /// <summary>
    /// Sets the source range of the AST node.
    /// </summary>
    /// <returns>A new instance of the AST node with the updated range.</returns>
    public AstNode WithRange(AstNode node, Token token)
    {
        return WithRange(node.Range.Document, node.Range.Start, token.GetSourceSpanEnd());
    }

    /// <summary>
    /// Sets the source range of the AST node using the provided document and span.
    /// </summary>
    /// <param name="document">The source document.</param>
    /// <param name="start">The start position of the range.</param>
    /// <param name="end">The end position of the range.</param>
    /// <returns>A new instance of the AST node with the updated range.</returns>
    public AstNode WithRange(SourceDocument document, SourceSpan start, SourceSpan end)
    {
        Range = new SourceRange(document, start, end);

        return this;
    }

    /// <summary>
    /// Sets the source range of the AST node using the start and end tokens.
    /// </summary>
    /// <param name="start">The start token providing the start position of the range.</param>
    /// <param name="end">The end token providing the end position of the range.</param>
    /// <returns>A new instance of the AST node with the updated range.</returns>
    public AstNode WithRange(Token start, Token end)
    {
        Range = new SourceRange(start.Document, start.GetSourceSpanStart(), end.GetSourceSpanEnd());

        return this;
    }

    /// <summary>
    /// Accepts a visitor for processing the AST node and returns a result of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of result returned by the visitor.</typeparam>
    /// <param name="visitor">The visitor to accept.</param>
    /// <returns>The result of processing the AST node with the visitor.</returns>
    public T Accept<T>(NodeVisitor<T> visitor) => visitor.Visit(this);

    /// <summary>
    /// Accepts a visitor for processing the AST node.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    public void Accept(NodeVisitor visitor) => visitor.Visit(this);

    /// <summary>
    /// Accepts a tagged visitor for processing the AST node and returns a result of type <typeparamref name="TReturn"/>.
    /// </summary>
    /// <typeparam name="TReturn">The type of result returned by the visitor.</typeparam>
    /// <typeparam name="TTag">The type of the tag provided to the visitor.</typeparam>
    /// <param name="visitor">The tagged visitor to accept.</param>
    /// <param name="tag">The tag to provide to the visitor.</param>
    /// <returns>The result of processing the AST node with the tagged visitor.</returns>
    public TReturn Accept<TReturn, TTag>(TaggedNodeVisitor<TReturn, TTag> visitor, TTag tag) => visitor.Visit(this, tag);

    /// <summary>
    /// Accepts a tagged visitor for processing the AST node.
    /// </summary>
    /// <typeparam name="TTag">The type of the tag provided to the visitor.</typeparam>
    /// <param name="visitor">The tagged visitor to accept.</param>
    /// <param name="tag">The tag to provide to the visitor.</param>
    public void Accept<TTag>(TaggedNodeVisitor<TTag> visitor, TTag tag) => visitor.Visit(this, tag);

    /// <summary>
    /// Adds a message to the document with the current node range
    /// </summary>
    /// <param name="severity"></param>
    /// <param name="message"></param>
    public void AddMessage(MessageSeverity severity, string message)
    {
        Range.Document.Messages.Add(new Message(severity, message, Range));
    }

    /// <summary>
    /// Adds a message to the document and uses a token for a location
    /// </summary>
    /// <param name="severity"></param>
    /// <param name="message"></param>
    public void AddMessage(MessageSeverity severity, string message, Token token)
    {
        Range.Document.Messages.Add(new Message(severity, message, token.GetRange()));
    }

    /// <summary>
    /// Set a tag
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public AstNode WithTag(object tag)
    {
        Tag = tag;

        return this;
    }

    /// <summary>
    /// Get the parent as specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? GetParentAs<T>()
        where T : AstNode
    {
        return Parent as T;
    }

    /// <summary>
    /// Checks if the node has a specific tag set
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool HasTag(object tag)
    {
        return Tag == tag;
    }

    /// <summary>
    /// Get the tag as type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? GetTag<T>()
    {
        return (T?)Tag;
    }
}
