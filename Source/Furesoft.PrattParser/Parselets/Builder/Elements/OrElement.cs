using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class OrElement: BinaryElement
{
    public OrElement(SyntaxElement first, SyntaxElement second) : base(first, second)
    {
    }

    public override bool Parse(Parser parser, List<(string, AstNode)> result)
    {
        First.CurrentToken = CurrentToken;

        return First.Parse(parser, result) || Second.Parse(parser, result);
    }

    public override string ToString()
    {
        return $"{First} | {Second}";
    }
}
