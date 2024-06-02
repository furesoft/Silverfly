using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class AndElement(SyntaxElement first, SyntaxElement second) : SyntaxElement
{
    public SyntaxElement First { get; } = first;
    public SyntaxElement Second { get; } = second;

    public override void Parse(Parser parser, List<AstNode> result)
    {
        First.CurrentToken = CurrentToken;

        First.Parse(parser, result);
        Second.Parse(parser, result);
    }
}
