using Silverfly.Repl;
using Silverfly.Sample.Func.Values;
using Silverfly.Text;

namespace Silverfly.Sample.Func;

class Repl() : ReplInstance<ExpressionGrammar>(new FuncPromptCallbacks())
{
    public override void Evaluate(string input)
    {
        var parsed = Parser.Parse(input, "repl.f");
        var rewritten = parsed.Tree.Accept(new RewriterVisitor());

        //Console.WriteLine(rewritten.Accept(new PrintVisitor()));

        if (parsed.Document.Messages.Count == 0)
        {
            var evaluated = rewritten.Accept(new EvaluationVisitor(), Scope.Root);

            if (evaluated is NameValue n)
            {
                rewritten.AddMessage(MessageSeverity.Error, $"Symbol '{n.Name}' not defined");
            }
        }
    }
}
