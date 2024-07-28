using Silverfly.Parselets;
using Silverfly.Sample.Func.Parselets;

namespace Silverfly.Sample.Func;

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
        Register("enum", new EnumParselet());
        Postfix("!");
        InfixLeft(".", DefaultPrecedenceLevels.Call);
        InfixLeft("=", DefaultPrecedenceLevels.Assignment);
        InfixLeft("..", "Exponent");

        Register("[", new ListValueParselet());

        Register("->", new LambdaParselet());

        Register("if", new IfParselet());
        Register("import", new ImportParselet());
        Register("module", new ModuleParselet());
        Register("@", new AnnotationParselet());

        Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            seperator: PredefinedSymbols.EOL);
    }
}
