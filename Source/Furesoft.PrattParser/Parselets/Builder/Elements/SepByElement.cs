using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class SepByElement(string name, Symbol seperator, Symbol terminator) : SyntaxElement
{
    public override void Parse(Parser parser, List<(string, AstNode)> result)
    {
        var parsed = parser.ParseSeperated(seperator, terminator);

        result.Add((name, new BlockNode(seperator, parsed)));
    }
}
