namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class AndElement(SyntaxElement first, SyntaxElement second) : SyntaxElement
{
    public override void Parse(Parser parser)
    {
        first.Parse(parser);
        second.Parse(parser);
    }
}
