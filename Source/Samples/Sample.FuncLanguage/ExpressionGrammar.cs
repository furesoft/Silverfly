using Silverfly;
using Silverfly.Parselets;
using Sample.FuncLanguage.Parselets;

namespace Sample.FuncLanguage;

class ExpressionGrammar : Parser
{
    protected override void InitLexer(Lexer lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.MatchNumber(allowHex: false, allowBin: false);
        lexer.UseNameAdvancer(new SampleNameAdvancer());
        lexer.MatchString("\"", "\"");
    }

    protected override void InitParselets()
    {
        AddCommonLiterals();
        AddArithmeticOperators();

        Register("(", new CallParselet(PrecedenceLevels.GetPrecedence("Call")));
        Register("(", new TupleOrGroupParselet());

        Register(PredefinedSymbols.Name, new NameParselet());

        Register("let", new VariableBindingParselet());
        Postfix("!");
        InfixLeft(".", "Call");

        Register("[", new ListValueParselet());

        Register("->", new LambdaParselet());

        Register("if", new IfParselet());
        Register("import", new ImportParselet());
        Register("module", new ModuleParselet());

        Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            seperator: PredefinedSymbols.Semicolon);
    }
}
