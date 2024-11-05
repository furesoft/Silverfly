using Silverfly.Lexing.IgnoreMatcher.Comments;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Parselets;

namespace Silverfly.Sample.Func;

class ExpressionGrammar : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.AddKeywords("let", "if", "then", "else", "enum", "import", "module"); //mark this symbols as keyword for syntax highlighting
        
        lexer.IgnoreWhitespace();
        lexer.MatchNumber(allowHex: false, allowBin: false);
        lexer.UseNameAdvancer(new SampleNameAdvancer());
        lexer.MatchString("\"", "\"");

        lexer.Ignore(new SingleLineCommentIgnoreMatcher("//"));
        lexer.Ignore(new MultiLineCommentIgnoreMatcher("/*", "*/"));
        
        lexer.MatchPattern("#test", "%.*%");
        
        lexer.IgnorePattern(@"\?");

        lexer.AddSymbols("/*", "*/", "//", "\"", "->", "+", "-", "*", "/");
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.PrecedenceLevels.AddPrecedence("Range");

        def.AddCommonLiterals();
        def.AddArithmeticOperators();

        def.Register("(", new CallParselet(def.PrecedenceLevels.GetPrecedence("Call")));
        def.Register("(", new TupleOrGroupParselet());

        def.Register(PredefinedSymbols.Name, new NameParselet());

        def.Register("let", new VariableBindingParselet());
        def.Register("enum", new EnumParselet());
        def.Postfix("!");
        def.InfixLeft(".", DefaultPrecedenceLevels.Call);
        def.InfixRight("=", DefaultPrecedenceLevels.Assignment);
        def.InfixLeft("..", "Range");
        def.Register("[", new IndexParselet(def.PrecedenceLevels.GetPrecedence("Range")));

        def.Register("[", new ListValueParselet());

        def.Register("->", new LambdaParselet());

        def.Register("if", new IfParselet());
        def.Register("import", new ImportParselet());
        def.Register("module", new ModuleParselet());
        def.Register("@", new AnnotationParselet());
        //parserDefinition.Register("def", new GeneratedParselet());

        def.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF,
            separator: PredefinedSymbols.EOL);
    }
}
