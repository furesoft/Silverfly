using Sample.JSON.Parselets;
using Silverfly;

namespace Sample.JSON;

public class JsonGrammar : Parser
{
    protected override void InitLexer(Lexer lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.MatchBoolean();
        lexer.MatchNumber(false, false);
        lexer.MatchString("\"", "\"");
    }

    protected override void InitParselets()
    {
        AddCommonLiterals();

        Register("{", new ObjectParselet());
    }
}
