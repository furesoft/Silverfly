using System.Linq.Expressions;
using Furesoft.PrattParser;
using Furesoft.PrattParser.Lexing.IgnoreMatcher.Comments;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using static Furesoft.PrattParser.Parselets.Builder.Helpers;

namespace TestProject;

public class TestParser : Parser
{
    protected override void Init()
    {
        Register(PredefinedSymbols.Name, new NameParselet());

        Register("(", new CallParselet());

        Ternary("?", ":", BindingPower.Conditional);

        this.AddArithmeticOperators();
        this.AddBitOperators();
        this.AddLogicalOperators();
        this.AddCommonLiterals();
        this.AddCommonAssignmentOperators();

        Prefix("not", BindingPower.Prefix);

        Postfix("!", BindingPower.PostFix);

        InfixRight("^", BindingPower.Exponent);

        InfixLeft("->", BindingPower.Product);

        Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            seperator: PredefinedSymbols.Semicolon);

        StmtBuilder<IfNode>(
            "if" + expr("Cond") + "then" +
                expr("Body") +
            "else" +
                expr("ElseBody"));
    }

    protected override void InitLexer(Lexer lexer)
    {
        lexer.Ignore(' ', '\r', '\t');
        lexer.Ignore("\r\n");

        lexer.MatchBoolean();
        lexer.MatchString("'", "'");
        lexer.MatchNumber(allowHex: true, allowBin: true);

        lexer.Ignore(new SingleLineCommentIgnoreMatcher(PredefinedSymbols.SlashSlash));
        lexer.Ignore(
            new MultiLineCommentIgnoreMatcher(PredefinedSymbols.SlashAsterisk, PredefinedSymbols.AsteriskSlash));
    }
}
