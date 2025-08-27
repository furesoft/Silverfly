using Silverfly.Sample.JSON.Nodes;
using Silverfly.Sample.JSON.Parselets;

namespace Silverfly.Sample.JSON;

public class JsonGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.AddKeywords("null", "true", "false");
        
        lexer.IgnoreWhitespace();
        
        lexer.MatchBoolean();
        lexer.MatchNumber(false, false);
        lexer.MatchString("\"", "\"");

        lexer.AddSymbols(":", "{", "}");
    }

    protected override void InitParser(ParserDefinition parser)
    {
        parser.AddCommonLiterals();

        parser.Register("{", new ObjectParselet());
        parser.Register("null", new NullParselet());
        parser.Register("[", new JsonArrayParselet());
    }
}
