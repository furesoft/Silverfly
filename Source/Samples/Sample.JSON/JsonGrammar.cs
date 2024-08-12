using Sample.JSON.Nodes;
using Sample.JSON.Parselets;
using Silverfly;

namespace Sample.JSON;

public class JsonGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.AddKeywords("null", "true", "false");
        
        lexer.IgnoreWhitespace();
        
        lexer.MatchBoolean();
        lexer.MatchNumber(false, false);
        lexer.MatchString("\"", "\"");
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.AddCommonLiterals();

        def.Register("{", new ObjectParselet());
        def.Register("null", new NullParselet());
        def.Register("[", new JsonArrayParselet());
    }
}
