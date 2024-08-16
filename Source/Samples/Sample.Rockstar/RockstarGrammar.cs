using Sample.Rockstar.Matchers;
using Sample.Rockstar.Parselets;
using Silverfly;
using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;
using static Silverfly.PredefinedSymbols;

namespace Sample.Rockstar;

public class RockstarGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.AddSymbols("(", ")");
        lexer.AddKeywords(AliasedBooleanMatcher.TrueAliases);
        lexer.AddKeywords(AliasedBooleanMatcher.FalseAliases);
        lexer.AddKeywords(EmptyStringMatcher.Aliases);
        
        lexer.UseNameAdvancer(new RockstarNameAdvancer());
        lexer.AddMatcher(new AliasedBooleanMatcher());
        lexer.MatchNumber(false,false);
        lexer.AddMatcher(new EmptyStringMatcher());
        
        lexer.IgnoreWhitespace();
        lexer.Ignore(new MultiLineCommentIgnoreMatcher("(", ")"));
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Block(SOF, EOF,
            separator: EOL);
        
        def.Register(PredefinedSymbols.Boolean, new AliasedBooleanParselet());
        def.Register("#empty_string", new EmptyStringParselet());
        def.Register(Name, new NameParselet());
    }
}
