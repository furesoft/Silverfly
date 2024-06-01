using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder;

public abstract class SyntaxElement {
    public abstract void Parse(Parser parser);

    public static SyntaxElement operator +(SyntaxElement first, SyntaxElement second) {
        return new AndElement(first, second);
    }
}

public class AndElement(SyntaxElement first, SyntaxElement second) : SyntaxElement
{
    public override void Parse(Parser parser)
    {
        first.Parse(parser);
        second.Parse(parser);
    }
}