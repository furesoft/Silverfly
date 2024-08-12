using Sample.JSON.Nodes;
using Sample.JSON.Parselets;
using Silverfly;

namespace Sample.JSON;

public class JsonGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.MatchBoolean();
        lexer.MatchNumber(false, false);
        lexer.MatchString("\"", "\"");
        lexer.AddKeywords("null", "true", "false");
    }

    protected override void InitParser(ParserDefinition parserDefinition)
    {
        parserDefinition.AddCommonLiterals();

        parserDefinition.Register("{", new ObjectParselet());
        parserDefinition.Register("null", new NullParselet());
        parserDefinition.Register("[", new JsonArrayParselet());
    }
}
