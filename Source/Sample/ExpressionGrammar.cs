using Silverfly;
using Silverfly.Parselets;
using Sample.Parselets;

namespace Sample;

class ExpressionGrammar : Parser
{
    protected override void InitLexer(Lexer lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.MatchNumber(allowHex: false, allowBin: false);
        lexer.UseNameAdvancer(new SampleNameAdvancer());
    }

    protected override void InitParselets()
    {
        AddCommonLiterals();
        AddArithmeticOperators();

        Register("(", new CallParselet(BindingPowers.Get("Call")));
        Register(PredefinedSymbols.Name, new NameParselet());

        Register("let", new VariableBindingParselet());
        Postfix("!");

        Register("()", new UnitValueParselet());
        Register("[", new ListValueParselet());

        Register("->", new LambdaParselet());

        Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            seperator: PredefinedSymbols.Semicolon);
    }
}
