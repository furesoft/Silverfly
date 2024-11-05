using Silverfly;
using Silverfly.Helpers;
using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Lexing.Matcher;
using Silverfly.Parselets;

namespace TestProject;

public class TestParser : Parser
{
    protected override void InitParser(ParserDefinition def)
    {
        def.Typename("<", ">", ",");
        def.Register(PredefinedSymbols.Name, new NameParselet());

        def.Register("(", new CallParselet(def.PrecedenceLevels.GetPrecedence("Call")));

        def.Ternary("?", ":", "Conditional");
        def.InfixLeft(".", "Call");

        def.AddArithmeticOperators();
        def.AddBitOperators();
        def.AddLogicalOperators();
        def.AddCommonLiterals();
        def.AddCommonAssignmentOperators();

        def.Prefix("not");
        def.Postfix("!");

        def.InfixRight("^", "Exponent");

        def.InfixLeft("->", "Product");
        def.InfixLeft(">>", "Product");
        def.InfixLeft("<<", "Product");

        def.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            separator: ';');
    }

    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.Ignore("\r", "\r\n");

        lexer.Context<TypenameContext>("<");

        lexer.AddSymbols("<", ">");
        lexer.AddSymbols("<<", ">>");

        lexer.AddSymbol(",");

        lexer.MatchBoolean();
        lexer.MatchString("'", "'");
        lexer.MatchNumber(allowHex: true, allowBin: true);

        lexer.Ignore(new SingleLineCommentIgnoreMatcher(PredefinedSymbols.SlashSlash));
        lexer.Ignore(
            new MultiLineCommentIgnoreMatcher(PredefinedSymbols.SlashAsterisk, PredefinedSymbols.AsteriskSlash));
    }
}
