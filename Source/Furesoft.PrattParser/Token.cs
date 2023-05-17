namespace Furesoft.PrattParser;

/// <summary>
/// A simple token class. These are generated by <see cref="Lexer"/>
/// and consumed by <see cref="Parser"/>.
/// </summary>
public sealed class Token
{
    public Symbol Type { get; }

    public string Text { get; }

    public int Line { get; }

    public int Column { get; }

    public SourceDocument Document { get; set; }

    public Token(Symbol type, string text, int line, int column)
    {
        Type = type;
        Text = text;
        Line = line;
        Column = column;
    }

    public override string ToString() { return Text; }

    public SourceSpan GetSourceSpanStart()
    {
        return new(Line, Column);
    }

    public SourceSpan GetSourceSpanEnd()
    {
        return new(Line, Column + Text.Length);
    }

    public Token WithDocument(SourceDocument document)
    {
        this.Document = document;
        
        return this;
    }
}
