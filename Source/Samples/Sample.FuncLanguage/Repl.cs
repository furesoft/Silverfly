using MrKWatkins.Ast.Processing;
using Silverfly.Nodes;
using Silverfly.Repl;

namespace Silverfly.Sample.Func;

internal class Repl : ReplInstance<ExpressionGrammar, FuncPromptCallbacks>
{
    protected override void Evaluate(string input)
    {
        var parsed = Parser.Parse(input, "repl.f");
        var pipeline = Pipeline<AstNode>.Build(_ =>
            _.AddStage<LiteralReplacer>()
        );
        pipeline.Run(parsed.Tree);
        //Console.WriteLine(rewritten.Accept(new PrintVisitor()));

        if (parsed.Document.Messages.Count == 0)
        {
            var context = new EvaluationContext { Scope = Scope.Root };
            EvaluationListener.Listener.Listen(context, parsed.Tree);
        }
    }
}
