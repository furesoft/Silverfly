using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Parselets;

namespace Silverfly.Sample.Func;

class ExpressionGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.MatchNumber(allowHex: false, allowBin: false);
        lexer.UseNameAdvancer(new SampleNameAdvancer());
        lexer.MatchString("\"", "\"");

        lexer.Ignore(new SingleLineCommentIgnoreMatcher("//"));
        lexer.Ignore(new MultiLineCommentIgnoreMatcher("/*", "*/"));
    }

    protected override void InitParser(ParserDefinition parserDefinition)
    {
        parserDefinition.PrecedenceLevels.AddPrecedence("Range");

        parserDefinition.AddCommonLiterals();
        parserDefinition.AddArithmeticOperators();

        parserDefinition.Register("(", new CallParselet(parserDefinition.PrecedenceLevels.GetPrecedence("Call")));
        parserDefinition.Register("(", new TupleOrGroupParselet());

        parserDefinition.Register(PredefinedSymbols.Name, new NameParselet());

        parserDefinition.Register("let", new VariableBindingParselet());
        parserDefinition.Register("enum", new EnumParselet());
        parserDefinition.Postfix("!");
        parserDefinition.InfixLeft(".", DefaultPrecedenceLevels.Call);
        parserDefinition.InfixLeft("=", DefaultPrecedenceLevels.Assignment);
        parserDefinition.InfixLeft("..", "Range");
        parserDefinition.Register("[", new IndexParselet(parserDefinition.PrecedenceLevels.GetPrecedence("Range")));

        parserDefinition.Register("[", new ListValueParselet());

        parserDefinition.Register("->", new LambdaParselet());

        parserDefinition.Register("if", new IfParselet());
        parserDefinition.Register("import", new ImportParselet());
        parserDefinition.Register("module", new ModuleParselet());
        parserDefinition.Register("@", new AnnotationParselet());

        parserDefinition.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            separator: PredefinedSymbols.EOL);
    }
}
