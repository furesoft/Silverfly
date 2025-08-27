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

    protected override void InitParser(ParserDefinition parser)
    {
        parser.Register(new OperationParselet(), "+", "-", "<", ">", ".", ",");
        parser.Register("[", new LoopParselet());

        parser.Block(PredefinedSymbols.SOF, PredefinedSymbols.EOF);
    }
}
