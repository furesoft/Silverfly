using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;
using Silverfly.Parselets.Literals;
using Silverfly.Sample.Rockstar.Matchers;
using Silverfly.Sample.Rockstar.Parselets;
using static Silverfly.PredefinedSymbols;

namespace Silverfly.Sample.Rockstar;

public class RockstarGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreCasing = true;
        
        lexer.AddSymbols("(", ")");
        var emptyStringMatcher = new MappingMatcher("#empty_string", ["empty", "silent", "silence"]);
        var nullStringMatcher = new MappingMatcher("#null", ["null", "nothing", "nowhere", "nobody", "gone"]);
        var pronounMatcher = new MappingMatcher("#pronoun", ["it", "he", "she", "him", "her", "they", "them", "ze", "hir", "zie", "zir", "xe", "xem", "ve", "ver"]);
        var poeticLiteralMatcher = new MappingMatcher("#poetic", ["is", "are", "was", "were"]);

        lexer.AddKeywords(AliasedBooleanMatcher.TrueAliases);
        lexer.AddKeywords(AliasedBooleanMatcher.FalseAliases);
        lexer.AddKeywords(emptyStringMatcher.Aliases);
        lexer.AddKeywords(pronounMatcher.Aliases);
        lexer.AddKeywords(poeticLiteralMatcher.Aliases);
        lexer.AddKeywords(PrintParselet.Aliases);
        lexer.AddKeywords("let", "be", "put", "into");
        
        lexer.AddSymbol(Environment.NewLine + Environment.NewLine); //blank lines
        lexer.AddSymbol(Environment.NewLine);

        lexer.MatchNumber(false,false);

        lexer.UseNameAdvancer(new RockstarNameAdvancer());

        lexer.AddMatcher(new AliasedBooleanMatcher());
        lexer.AddMatcher(emptyStringMatcher);
        lexer.AddMatcher(nullStringMatcher);
        lexer.AddMatcher(pronounMatcher);
        lexer.AddMatcher(poeticLiteralMatcher);
        
        lexer.Ignore(" ");
        lexer.Ignore(new MultiLineCommentIgnoreMatcher("(", ")"));
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Block(SOF, EOF, separator: EOL);
        
        def.Register(PredefinedSymbols.Boolean, new AliasedBooleanParselet());
        def.Register("#empty_string", new MappingParselet(""));
        def.Register("#null", new MappingParselet(null));
        def.Register("#pronoun", new MappingParselet(null));
        
        def.Register(new AssignmentParselet(), "let", "put");
        def.Register("#poetic", new PoeticLiteralParselet());

        def.Register(Name, new NameParselet());
        def.Register(Number, new NumberParselet());
        
        def.Register(new PrintParselet(), PrintParselet.Aliases);
        
        def.Block("if", "#line");
    }
}
