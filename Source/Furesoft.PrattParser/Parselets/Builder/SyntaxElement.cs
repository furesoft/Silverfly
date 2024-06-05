using System.Collections.Generic;
using Furesoft.PrattParser.Parselets.Builder.Elements;

namespace Furesoft.PrattParser.Parselets.Builder;

public abstract class SyntaxElement
{
    public Token CurrentToken;
    public abstract bool Parse(Parser parser, List<(string, object)> result);

    public static SyntaxElement operator +(SyntaxElement first, SyntaxElement second)
    {
        return new AndElement(first, second);
    }

    public static SyntaxElement operator |(SyntaxElement first, SyntaxElement second)
    {
        return new OrElement(first, second);
    }

    public static implicit operator SyntaxElement(string keyword)
    {
        return new KeywordElement(keyword);
    }
}
