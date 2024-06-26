using Furesoft.PrattParser;
using Furesoft.PrattParser.Parselets;

namespace Sample;

class ExpressionGrammar : Parser
{
    protected override void InitLexer(Lexer lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.MatchNumber(allowHex: false, allowBin: false);
    }

    protected override void InitParselets()
    {
        AddCommonLiterals();
        AddArithmeticOperators();

        Register("(", new CallParselet(BindingPowers.Get("Call")));
        Register(PredefinedSymbols.Name, new NameParselet());

        Register("let", new VariableBindingParselet());
    }
}
