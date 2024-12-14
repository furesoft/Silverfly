using Silverfly.Repl;
using Silverfly.Sample.Rockstar.Evaluation;

namespace Silverfly.Sample.Rockstar;

public class Repl : ReplInstance<RockstarGrammar, RockstarCallbacks>
{
    protected override void Evaluate(string input)
    {
        var tu = Parser.Parse(input, null);

        var context = new EvaluationContext { Scope = Scope.Root };

        EvaluationListener.Listener.Listen(context, tu.Tree);
    }
}
