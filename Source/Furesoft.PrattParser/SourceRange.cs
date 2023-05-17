namespace Furesoft.PrattParser;

public struct SourceRange
{
    public SourceRange(SourceSpan start, SourceSpan end)
    {
        Start = start;
        End = end;
    }
    
    public SourceSpan Start { get; }
    public SourceSpan End { get; }
    public static SourceRange Empty { get; } = new();

    public override string ToString()
    {
        return $"{Start} - {End}";
    }
}
