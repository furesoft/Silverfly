using Silverfly;
using Silverfly.Repl;

namespace Sample.JSON;

public class Repl() : ReplInstance<JsonGrammar>
{
    protected override void Evaluate(string input)
    {
        var parsed = Parser.Parse(input);
        Console.WriteLine(parsed.Tree.Accept(new PrintVisitor()));
    }
}
