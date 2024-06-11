using Furesoft.PrattParser;
using Furesoft.PrattParser.Lexing.IgnoreMatcher.Comments;
using Furesoft.PrattParser.Parselets;
using static Furesoft.PrattParser.Parselets.Builder.Helpers;

namespace TestProject;

public class TestParser : Parser
{
    protected override void InitParselets()
    {
        Register(PredefinedSymbols.Name, new NameParselet());

        Register("(", new CallParselet(BindingPowers.Get("Call").Id));

        Ternary("?", ":", "Conditional");

        this.AddArithmeticOperators();
        this.AddBitOperators();
        this.AddLogicalOperators();
        this.AddCommonLiterals();
        this.AddCommonAssignmentOperators();

        Prefix("not");

        Postfix("!");

        InfixRight("^", "Exponent");

        InfixLeft("->", "Product");

        Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            seperator: PredefinedSymbols.Semicolon);

        StmtBuilder<IfNode>(
            "if" + expr("Cond") + "then" +
                expr("Body") +
            "else" +
                expr("ElseBody"));

        ExprBuilder<IfNode>(
            (keyword("ifnt") | "unless") + expr("Cond") + "then" +
                expr("Body")
        );
    }

    protected override void InitLexer(Lexer lexer)
    {
        lexer.Ignore(' ', '\r', '\t');
        lexer.Ignore("\r\n");

        lexer.MatchBoolean(ignoreCasing: true);
        lexer.MatchString("'", "'");
        lexer.MatchNumber(allowHex: true, allowBin: true);

        lexer.Ignore(new SingleLineCommentIgnoreMatcher(PredefinedSymbols.SlashSlash));
        lexer.Ignore(
            new MultiLineCommentIgnoreMatcher(PredefinedSymbols.SlashAsterisk, PredefinedSymbols.AsteriskSlash));
    }
}
