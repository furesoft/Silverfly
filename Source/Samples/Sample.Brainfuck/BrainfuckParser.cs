using Sample.Brainfuck.Parselets;
using Silverfly;

namespace Sample.Brainfuck;

public class BrainfuckParser : Parser
{
    protected override void InitLexer(LexerConfig lexer)
    {

    }

    protected override void InitParser(ParserDefinition def)
    {
        def.Group("[", "]");

        def.Register(new OperationParselet(), "+", "-", "<", ">", ".", ",");
    }
}
