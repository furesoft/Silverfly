using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class AndElement : BinaryElement
{
    public AndElement(SyntaxElement first, SyntaxElement second) : base(first, second)
    {
    }

    public override bool Parse(Parser parser, List<(string, AstNode)> result)
    {
        First.CurrentToken = CurrentToken;

        if (First.Parse(parser, result))
        {
            if (Second.Parse(parser, result))
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return $"{First} {Second}";
    }
}
