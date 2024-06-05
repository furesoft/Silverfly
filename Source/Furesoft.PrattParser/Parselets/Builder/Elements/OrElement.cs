using System.Collections.Generic;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class OrElement(SyntaxElement first, SyntaxElement second) : BinaryElement(first, second)
{
    public override bool Parse(Parser parser, List<(string, object)> result)
    {
        First.CurrentToken = CurrentToken;
        Second.CurrentToken = CurrentToken;

        var parseResult = First.Parse(parser, result) || Second.Parse(parser, result);
        PropagateMessages();

        if (!parseResult)
        {
            Messages.Add(Message.Error($"Expected '{First}' or '{Second}'"));
        }

        return parseResult;
    }

    public override string ToString()
    {
        return $"({First} | {Second})";
    }
}
