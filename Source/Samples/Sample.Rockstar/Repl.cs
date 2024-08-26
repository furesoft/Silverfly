using Sample.Rockstar.Evaluation;
using Silverfly.Repl;

namespace Sample.Rockstar;

public class Repl : ReplInstance<RockstarGrammar, RockstarCallbacks>
{
    protected override void Evaluate(string input)
    {
        var tu = Parser.Parse(input);
        tu.Tree.Accept(new EvaluationVisitor(), Scope.Root);
    }
}
