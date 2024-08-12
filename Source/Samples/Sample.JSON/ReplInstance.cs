using Silverfly.Generator;
using Silverfly.Repl;

namespace Sample.JSON;

public class Repl() : ReplInstance<JsonGrammar>
{
    protected override void Evaluate(string input)
    {
        var parsed = Parser.Parse(input);
        parsed.Tree.Accept(new JsonPrinter());
    }
}
