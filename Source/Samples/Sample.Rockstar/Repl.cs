using Silverfly.Repl;
using Silverfly.Sample.Rockstar.Evaluation;

namespace Silverfly.Sample.Rockstar;

public class Repl : ReplInstance<RockstarGrammar, RockstarCallbacks>
{
    protected override void Evaluate(string input)
    {
        var tu = Parser.Parse(input, null);
        tu.Tree.Accept(new EvaluationVisitor(), Scope.Root);
    }
}
