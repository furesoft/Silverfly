using Silverfly;
using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;

namespace TestProject;

public class TestParser : Parser
{
    protected override void InitParser(ParserDefinition parserDefinition)
    {
        parserDefinition.Register(PredefinedSymbols.Name, new NameParselet());

        parserDefinition.Register("(", new CallParselet(parserDefinition.PrecedenceLevels.GetPrecedence("Call")));

        parserDefinition.Ternary("?", ":", "Conditional");
        parserDefinition.InfixLeft(".", "Call");

        parserDefinition.AddArithmeticOperators();
        parserDefinition.AddBitOperators();
        parserDefinition.AddLogicalOperators();
        parserDefinition.AddCommonLiterals();
        parserDefinition.AddCommonAssignmentOperators();

        parserDefinition.Prefix("not");
        parserDefinition.Postfix("!");

        parserDefinition.InfixRight("^", "Exponent");

        parserDefinition.InfixLeft("->", "Product");

        parserDefinition.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            separator: PredefinedSymbols.Semicolon);
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
