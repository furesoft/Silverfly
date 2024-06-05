using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class KeywordElement(string keyword, string name = null) : SyntaxElement
{
    public string Keyword { get; } = keyword;

    public override bool Parse(Parser parser, List<(string, object)> result)
    {
        if (CurrentToken != default)
        {
            if (CurrentToken.Type != (Symbol)Keyword)
            {
                return false;
            }
        }
        else
        {
            if (!parser.Match(Keyword))
            {
                return false;
            }
        }

        if (!string.IsNullOrEmpty(name))
        {
            result.Add((Keyword, CurrentToken));
        }

        return true;
    }

    public void AddErrorMessage(Parser parser)
    {
        parser.Document.Messages.Add(
                            Message.Error($"Expected '{Keyword}' got '{parser.LookAhead(0).Text}'"));
    }

    public override string ToString()
    {
        return Keyword;
    }
}
