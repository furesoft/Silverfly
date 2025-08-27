using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Parselets;

namespace Silverfly.Sample.Func;

internal class ExpressionGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.AddKeywords("let", "if", "then", "else", "enum", "import",
            "module"); //mark this symbols as keyword for syntax highlighting

        lexer.IgnoreWhitespace();
        lexer.MatchNumber(false, false);
        lexer.UseNameAdvancer(new SampleNameAdvancer());
        lexer.MatchString("\"", "\"");

        lexer.Ignore(new SingleLineCommentIgnoreMatcher("//"));
        lexer.Ignore(new MultiLineCommentIgnoreMatcher("/*", "*/"));

        lexer.MatchPattern("#test", "%.*%");

        lexer.IgnorePattern(@"\?");

        lexer.AddSymbols("/*", "*/", "//", "\"", "->", "+", "-", "*", "/");
    }

    protected override void InitParser(ParserDefinition parser)
    {
        parser.PrecedenceLevels.AddPrecedence("Range");

        parser.AddCommonLiterals();
        parser.AddArithmeticOperators();

        parser.Register("(", new CallParselet(parser.PrecedenceLevels.GetPrecedence("Call")));
        parser.Register("(", new TupleOrGroupParselet());

        parser.Register(PredefinedSymbols.Name, new NameParselet());

        parser.Register("let", new VariableBindingParselet());
        parser.Register("enum", new EnumParselet());
        parser.Postfix("!");
        parser.InfixLeft(".", DefaultPrecedenceLevels.Call);
        parser.InfixRight("=", DefaultPrecedenceLevels.Assignment);
        parser.InfixLeft("..", "Range");
        parser.Register("[", new IndexParselet(parser.PrecedenceLevels.GetPrecedence("Range")));

        parser.Register("[", new ListValueParselet());

        parser.Register("->", new LambdaParselet());

        parser.Register("if", new IfParselet());
        parser.Register("import", new ImportParselet());
        parser.Register("module", new ModuleParselet());
        parser.Register("@", new AnnotationParselet());

        parser.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            PredefinedSymbols.EOL);
    }
}
