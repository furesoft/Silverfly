using Silverfly.Repl;
using Silverfly.Sample.JSON;

namespace Sample.JSON;

public class Repl() : ReplInstance<JsonGrammar>
{
    protected override void Evaluate(string input)
    {
        var parsed = Parser.Parse(input);
        parsed.Tree.Accept(new JsonPrinter());
    }
}
