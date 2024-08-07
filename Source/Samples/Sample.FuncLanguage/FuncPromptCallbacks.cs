using PrettyPrompt.Completion;
using PrettyPrompt.Consoles;
using PrettyPrompt.Documents;
using PrettyPrompt.Highlighting;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;
using Silverfly.Repl;
using Silverfly.Sample.Func.Values;

namespace Silverfly.Sample.Func;

internal class FuncPromptCallbacks : ReplPromptCallbacks
{
    /*
    protected override IEnumerable<(KeyPressPattern Pattern, KeyPressCallbackAsync Callback)> GetKeyPressCallbacks()
    {
        // registers functions to be called when the user presses a key. The text
        // currently typed into the prompt, along with the caret position within
        // that text are provided as callback parameters.
        yield return (new(ConsoleModifiers.Control, ConsoleKey.F1), ShowFruitDocumentation);
    }
    */

    protected override async Task<(IReadOnlyList<OverloadItem>, int ArgumentIndex)> GetOverloadsAsync(string text, int caret, CancellationToken cancellationToken)
    {
        /*
        if (text == "print(")
        {
            var item = new OverloadItem("print(value)", "prints any value to the console", "returns unit", [new OverloadItem.Parameter("src", "The value to print")]);

            return ([item], 0);
        }
        */

        return (Array.Empty<OverloadItem>(), 0);
    }


    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(string text, int caret, TextSpan spanToBeReplaced, CancellationToken cancellationToken)
    {
        var typedWord = text.AsSpan(spanToBeReplaced.Start, spanToBeReplaced.Length).ToString();
        var tree = new ExpressionGrammar().Parse(text);
        var items = Scope.Root.Bindings.ToList();

        var scope = GetScope(tree.Tree, Scope.Root);

        if (scope is null)
        {
            return Task.FromResult((IReadOnlyList<CompletionItem>)Array.Empty<CompletionItem>());
        }

        if (scope.IsRoot)
        {
            items = scope.Bindings.ToList();//.Where(_ => !_.StartsWith("'") && !_.StartsWith("__"));
        }
        else
        {
            items = Keywords.Select(_ => new KeyValuePair<string, Value>(_, _)).ToList();
        }

        return Task.FromResult<IReadOnlyList<CompletionItem>>(
            items
                .Where(_ => _.Key.StartsWith(typedWord, StringComparison.InvariantCultureIgnoreCase))
                .Select(item =>
                {
                    var displayText = new FormattedString(item.Key,
                        new FormatSpan(0, item.Key.Length, AnsiColor.Blue));

                    var replacement = item.Key;

                    if (item.Value is LambdaValue)
                    {
                        replacement += "(";
                    }

                    return new CompletionItem(
                        replacementText: replacement,
                        displayText: displayText,
                        commitCharacterRules: [..new[]
                        {
                            new CharacterSetModificationRule(CharacterSetModificationKind.Add,
                                [.. Characters])
                        }]
                    );
                })
                .ToArray()
        );
    }

    private Scope GetScope(AstNode node, Scope scope)
    {
        if (node is BlockNode block)
        {
            if (block.Children.Count == 0)
            {
                return scope;
            }
            
            return GetScope(block.Children.First(), scope);
        }
        
        if (node is BinaryOperatorNode b && b.Operator == ".")
        {
            if (b.LeftExpr is NameNode nameNode && b.RightExpr is InvalidNode)
            {
                return scope.Get(nameNode.Name).Members;
            }
            else if (b.LeftExpr is NameNode n1 && b.RightExpr is NameNode n2)
            {
                var s = scope.Get(n1.Name);
                if (s is LambdaValue)
                {
                    return null;
                }

                return s.Get(n2.Name).Members;
            }
            else if (b.LeftExpr is BinaryOperatorNode ib && ib.Operator == ".")
            {
                return GetScope(b.LeftExpr, scope);
            }
        }

        return scope;
    }
}
