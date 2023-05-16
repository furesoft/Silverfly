namespace Furesoft.PrattParser;

public struct SourceRange
{
    public SourceSpan Start { get; set; }
    public SourceSpan End { get; set; }
    public static SourceRange Synthetic { get; } = new SourceRange();
}
