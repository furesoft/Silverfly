using Silverfly;
using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;

namespace TestProject;

public class TestParser : Parser
{
    protected override void InitParselets()
    {
        Register(PredefinedSymbols.Name, new NameParselet());

        Register("(", new CallParselet(PrecedenceLevels.GetPrecedence("Call")));

        Ternary("?", ":", "Conditional");
        InfixLeft(".", "Call");

        AddArithmeticOperators();
        AddBitOperators();
        AddLogicalOperators();
        AddCommonLiterals();
        AddCommonAssignmentOperators();

        Prefix("not");
        Postfix("!");

        InfixRight("^", "Exponent");

        InfixLeft("->", "Product");

        Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            seperator: PredefinedSymbols.Semicolon);
    }

    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.Ignore("\r", "\r\n");

        lexer.MatchBoolean(ignoreCasing: true);
        lexer.MatchString("'", "'");
        lexer.MatchNumber(allowHex: true, allowBin: true);

        lexer.Ignore(new SingleLineCommentIgnoreMatcher(PredefinedSymbols.SlashSlash));
        lexer.Ignore(
            new MultiLineCommentIgnoreMatcher(PredefinedSymbols.SlashAsterisk, PredefinedSymbols.AsteriskSlash));
    }
}
