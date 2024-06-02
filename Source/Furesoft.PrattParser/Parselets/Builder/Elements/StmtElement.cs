using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class StmtElement(string name) : SyntaxElement
{
    public override void Parse(Parser parser, List<(string, AstNode)> result)
    {
        result.Add((name, parser.ParseStatement()));
    }
}
