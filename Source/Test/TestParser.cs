using Furesoft.PrattParser;
using Furesoft.PrattParser.Matcher;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;

namespace Test;

public class TestParser : Parser<AstNode>
{
    public TestParser()
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

    protected override void InitLexer(Lexer lexer)
    {
        lexer.Ignore('\r');
        lexer.Ignore(' ');
        lexer.Ignore('\t');
        lexer.UseString("'","'");
        lexer.AddMatcher(new IntegerMatcher());
        lexer.AddMatcher(new SignedIntegerMatcher());
    }
}
