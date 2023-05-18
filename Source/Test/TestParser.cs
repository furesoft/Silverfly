using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Parselets.Literals;
using Furesoft.PrattParser.Parselets.Operators;

namespace Test;

public class TestParser : Parser<AstNode>
{
    public TestParser(Lexer lexer) : base(lexer)
    {
        Register(PredefinedSymbols.Name, new NameParselet());
        
        Register("(", new CallParselet());

        Ternary("?", ":", (int)BindingPower.Conditional);
        
        this.AddArithmeticOperators();
        this.AddBitOperators();
        this.AddLogicalOperators();
        this.AddCommonLiterals();
        this.AddCommonAssignmentOperators();

        Prefix("not", (int)BindingPower.Prefix);

        Postfix("!", (int)BindingPower.PostFix);
        
        InfixRight("^", (int)BindingPower.Exponent);
        
        InfixLeft("->", (int)BindingPower.Product);
    }
}
