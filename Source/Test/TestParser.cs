using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Parselets.Literals;

namespace Test;

public class TestParser : Parser<AstNode>
{
    public TestParser(Lexer lexer) : base(lexer)
    {
        Register(PredefinedSymbols.Name, new NameParselet());
        Register(PredefinedSymbols.Integer, new IntegerLiteralParselet());
        Register("=", new AssignParselet());

        Register("?", new ConditionalParselet());
        Register("(", new CallParselet());
        Group("(", ")");

        Prefix("+", (int)BindingPower.Prefix);
        Prefix("-", (int)BindingPower.Prefix);
        Prefix("~", (int)BindingPower.Prefix);
        Prefix("!", (int)BindingPower.Prefix);
        Prefix("not", (int)BindingPower.Prefix);

        Postfix("!", (int)BindingPower.PostFix);

        InfixLeft("+", (int)BindingPower.Sum);
        InfixLeft("-", (int)BindingPower.Sum);
        InfixLeft("*", (int)BindingPower.Product);
        InfixLeft("/", (int)BindingPower.Product);
        InfixRight("^", (int)BindingPower.Exponent);
        
        InfixLeft("->", (int)BindingPower.Product);
    }
}
