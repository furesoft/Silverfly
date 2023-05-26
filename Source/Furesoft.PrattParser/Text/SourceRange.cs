namespace Furesoft.PrattParser.Text;

public readonly struct SourceRange
{
    public SourceRange(SourceDocument document, SourceSpan start, SourceSpan end)
    {
        Document = document;
        Start = start;
        End = end;
    }

    public SourceDocument Document { get; }
    public SourceSpan Start { get; }
    public SourceSpan End { get; }
    public static SourceRange Empty { get; } = new();
    
    public bool Contains(int line, int column)
    {
        var inLeft = false;
        if (Start.Line < line) inLeft = true;
        else if (Start.Line > line) inLeft = false;
        else inLeft = Start.Column <= column;

        var inRight = false;
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
