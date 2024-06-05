namespace Furesoft.PrattParser.Text;

public readonly struct SourceRange(SourceDocument document, SourceSpan start, SourceSpan end)
{
    public SourceDocument Document { get; } = document;
    public SourceSpan Start { get; } = start;
    public SourceSpan End { get; } = end;
    public static SourceRange Empty { get; } = new();

    public static SourceRange From(SourceDocument document, int startLine, int startColumn, int endLine, int endColumn)
    {
        return new SourceRange(document, new SourceSpan(startLine, startColumn), new SourceSpan(endLine, endColumn));
    }

    public bool Contains(int line, int column)
    {
        bool inLeft;
        if (Start.Line < line) inLeft = true;
        else if (Start.Line > line) inLeft = false;
        else inLeft = Start.Column <= column;

        bool inRight;
        if (End.Line > line) inRight = true;
        else if (End.Line < line) inRight = false;
        else inRight = End.Column >= column;

        return inLeft && inRight;
    }

    public override string ToString()
    {
        return $"{Document.Filename}: {Start} - {End}";
    }
}
