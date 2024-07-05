using Silverfly;

namespace Sample.Xml;

public class XmlGrammar : Parser
{
    protected override void InitLexer(Lexer lexer)
    {
        lexer.IgnoreWhitespace();
    }

    protected override void InitParselets()
    {
    }
}