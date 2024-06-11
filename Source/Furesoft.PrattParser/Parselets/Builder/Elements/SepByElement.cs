using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class SepByElement(string name, Symbol seperator, Symbol terminator) : SyntaxElement
{
    public override bool Parse(Parser parser, List<(string, object)> result)
    {
        var parsed = parser.ParseSeperated(seperator, terminator);

        result.Add((name, new BlockNode(seperator, terminator).WithChildren(parsed)));

        return true;
    }

    public override string ToString()
    {
        return $"sepBy({seperator}, {terminator})";
    }
}