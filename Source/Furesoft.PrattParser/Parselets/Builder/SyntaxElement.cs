using Furesoft.PrattParser.Parselets.Builder.Elements;

namespace Furesoft.PrattParser.Parselets.Builder;

public abstract class SyntaxElement
{
    public abstract void Parse(Parser parser);

    public static SyntaxElement operator +(SyntaxElement first, SyntaxElement second)
    {
        return new AndElement(first, second);
    }

    public static implicit operator SyntaxElement(string keyword)
    {
        return new KeywordElement(keyword);
    }
}
