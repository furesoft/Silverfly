using Silverfly.Repl;

namespace Sample.Brainfuck;

public class Repl : ReplInstance<BrainfuckParser>
{
    protected override void Evaluate(string input)
    {
        var parsed = Parser.Parse(input);
        parsed.Tree.Accept(new EvalVisitor());
    }
}
