using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class KeywordElement(string keyword) : SyntaxElement
{
    public string Keyword { get; } = keyword;

    public override void Parse(Parser parser, List<(string, AstNode)> result)
    {
        if (CurrentToken != default)
        {
            if (CurrentToken.Type != (Symbol)Keyword)
            {
                AddErrorMessage(parser);
            }
        }
        else
        {
            if (!parser.Match(Keyword))
            {
                AddErrorMessage(parser);
            }
        }
    }

    private void AddErrorMessage(Parser parser)
    {
        parser.Document.Messages.Add(
                            Message.Error($"Expected '{Keyword}' got '{parser.LookAhead(0).Text}'"));
    }
}
