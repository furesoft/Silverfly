using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class ExprElement : SyntaxElement
{
    public override void Parse(Parser parser, List<AstNode> result)
    {
        result.Add(parser.Parse());
    }
}
