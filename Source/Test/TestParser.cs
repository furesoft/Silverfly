using Furesoft.PrattParser;
using Furesoft.PrattParser.Expressions;
using Furesoft.PrattParser.Parselets;

namespace Test;

public class TestParser : Parser<IAstNode>
{
    public TestParser(Lexer lexer) : base(lexer)
    {
        Register(PredefinedSymbols.Name, new NameParselet());
        Register("=", new AssignParselet());

        Register("?", new ConditionalParselet());
        Register("(", new CallParselet());
        Group("(", ")");

        Prefix("+", (int)BindingPower.Prefix);
        Prefix("-", (int)BindingPower.Prefix);
        Prefix("~", (int)BindingPower.Prefix);
        Prefix("!", (int)BindingPower.Prefix);

        Postfix("!", (int)BindingPower.PostFix);

        InfixLeft("+", (int)BindingPower.Sum);
        InfixLeft("-", (int)BindingPower.Sum);
        InfixLeft("*", (int)BindingPower.Product);
        InfixLeft("/", (int)BindingPower.Product);
        InfixRight("^", (int)BindingPower.Exponent);
    }
}
