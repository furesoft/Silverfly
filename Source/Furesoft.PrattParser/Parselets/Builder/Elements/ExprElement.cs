using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class ExprElement(string name) : SyntaxElement
{
    public override bool Parse(Parser parser, List<(string, AstNode)> result)
    {
        result.Add((name, parser.ParseExpression()));

        return true;
    }

    public override string ToString()
    {
        return "expr";
    }
}
