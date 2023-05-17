namespace Furesoft.PrattParser;

public struct SourceRange
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

    public override string ToString()
    {
        return $"{Document.Filename}: {Start} - {End}";
    }
}
