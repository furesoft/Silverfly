using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Parselets.Builder.Elements;

public class KeywordElement(string keyword) : SyntaxElement
{
    public override void Parse(Parser parser)
    {
        if (!parser.Match(keyword))
        {
            parser.Document.Messages.Add(
                Message.Error($"Expected '{keyword}' got '{parser.LookAhead(0).Text}'"));
        }
    }
}
