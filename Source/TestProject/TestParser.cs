using Silverfly;
using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;

namespace TestProject;

public class TestParser : Parser
{
    protected override void InitParser(ParserDefinition parser)
    {
        parser.Register(PredefinedSymbols.Name, new NameParselet());

        parser.Register("(", new CallParselet(parser.PrecedenceLevels.GetPrecedence("Call")));

        parser.Ternary("?", ":", "Conditional");
        parser.InfixLeft(".", "Call");

        parser.AddArithmeticOperators();
        parser.AddBitOperators();
        parser.AddLogicalOperators();
        parser.AddCommonLiterals();
        parser.AddCommonAssignmentOperators();

        parser.Prefix("not");
        parser.Postfix("!");

        parser.InfixRight("^", "Exponent");

        parser.InfixLeft("->", "Product");

        parser.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,";");
    }

    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.Ignore("\r", "\r\n");

        lexer.MatchBoolean();
        lexer.MatchString("'", "'");
        lexer.MatchNumber(true, true);

        lexer.Ignore(new SingleLineCommentIgnoreMatcher(PredefinedSymbols.SlashSlash));
        lexer.Ignore(
            new MultiLineCommentIgnoreMatcher(PredefinedSymbols.SlashAsterisk, PredefinedSymbols.AsteriskSlash));
    }
}
