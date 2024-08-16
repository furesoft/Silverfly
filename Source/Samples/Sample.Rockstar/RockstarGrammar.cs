using Silverfly;
using Silverfly.Lexing.IgnoreMatcher.Comments;

namespace Sample.Rockstar;

public class RockstarGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.AddSymbols("(", ")");
        
        lexer.IgnoreWhitespace();
        lexer.Ignore(new MultiLineCommentIgnoreMatcher("(", ")"));
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            separator: PredefinedSymbols.EOL);
    }
}
