using System.Collections.Generic;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class AndElement(SyntaxElement first, SyntaxElement second) : BinaryElement(first, second)
{
    public override bool Parse(Parser parser, List<(string, object)> result)
    {
        First.CurrentToken = CurrentToken;

        PropagateMessages();

        if (First.Parse(parser, result))
        {
            if (Second.Parse(parser, result))
            {
                return true;
            }
            else
            {
                Messages.Add(Message.Error($"Expected '{Second}'"));
            }
        }

        Messages.Add(Message.Error($"Expected '{First}'"));
        return false;
    }

    public override string ToString()
    {
        return $"{First} {Second}";
    }
}
