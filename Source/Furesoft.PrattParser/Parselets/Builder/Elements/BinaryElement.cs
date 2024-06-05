namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public abstract class BinaryElement(SyntaxElement first, SyntaxElement second) : SyntaxElement
{
    public SyntaxElement First { get; } = first;
    public SyntaxElement Second { get; } = second;

    protected void PropagateMessages()
    {
        Messages.AddRange(First.Messages);
        Messages.AddRange(Second.Messages);
    }
}
