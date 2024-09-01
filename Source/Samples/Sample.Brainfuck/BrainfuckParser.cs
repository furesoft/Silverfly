using Sample.Brainfuck.Parselets;
using Silverfly;
using Silverfly.Lexing.IgnoreMatcher.Comments;

namespace Sample.Brainfuck;

public class BrainfuckParser : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {
        lexer.IgnoreWhitespace();
        lexer.Ignore(new SingleLineCommentIgnoreMatcher("#"));
    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Register(new OperationParselet(), "+", "-", "<", ">", ".", ",");
        def.Register("[", new LoopParselet());

        def.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF);
    }
}
