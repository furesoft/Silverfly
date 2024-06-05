using System.Collections.Generic;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class OrElement(SyntaxElement first, SyntaxElement second) : BinaryElement(first, second)
{
    public override bool Parse(Parser parser, List<(string, object)> result)
    {
        First.CurrentToken = CurrentToken;
        Second.CurrentToken = CurrentToken;

        return First.Parse(parser, result) || Second.Parse(parser, result);
    }

    public override string ToString()
    {
        return $"({First} | {Second})";
    }
}
