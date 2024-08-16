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
        var emptyStringMatcher = new MappingMatcher("#empty_string", ["empty", "silent", "silence"]);
        var nullStringMatcher = new MappingMatcher("#null", ["null", "nothing", "nowhere", "nobody", "gone"]);
        var pronounMatcher = new MappingMatcher("#pronoun", ["it", "he", "she", "him", "her", "they", "them", "ze", "hir", "zie", "zir", "xe", "xem", "ve", "ver"]);

        lexer.AddKeywords(AliasedBooleanMatcher.TrueAliases);
        lexer.AddKeywords(AliasedBooleanMatcher.FalseAliases);
        lexer.AddKeywords(emptyStringMatcher.Aliases);
        lexer.AddKeywords(pronounMatcher.Aliases);

        lexer.MatchNumber(false,false);

        lexer.UseNameAdvancer(new RockstarNameAdvancer());

        lexer.AddMatcher(new AliasedBooleanMatcher());
        lexer.AddMatcher(emptyStringMatcher);
        lexer.AddMatcher(nullStringMatcher);
        lexer.AddMatcher(pronounMatcher);
        
        lexer.IgnoreWhitespace();
        lexer.Ignore(new MultiLineCommentIgnoreMatcher("(", ")"));
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Block(SOF, EOF,
            separator: EOL);
        
        def.Register(PredefinedSymbols.Boolean, new AliasedBooleanParselet());
        def.Register("#empty_string", new MappingParselet(""));
        def.Register("#null", new MappingParselet(null));
        def.Register("#pronoun", new MappingParselet(null));

        def.Register(Name, new NameParselet());
    }
}
