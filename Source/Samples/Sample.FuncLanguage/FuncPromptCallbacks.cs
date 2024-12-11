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

    protected override Task<IReadOnlyList<CompletionItem>> GetCompletionItemsAsync(string text, int caret, TextSpan spanToBeReplaced, CancellationToken cancellationToken)
    {
        var typedWord = text.AsSpan(spanToBeReplaced.Start, spanToBeReplaced.Length).ToString();
        var tree = Parser.Parse(text);
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
            items = Parser.Lexer.Config.Keywords.Select(_ => new KeyValuePair<string, Value>(_, _)).ToList();
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

        if (node is BinaryOperatorNode b && b.Operator.Text.Span == ".")
        {
            if (b.Left is NameNode nameNode && b.Right is InvalidNode)
            {
                return scope.Get(nameNode.Token.Text.ToString()).Members;
            }
            else if (b.Left is NameNode n1 && b.Right is NameNode n2)
            {
                var s = scope.Get(n1.Token.Text.ToString());
                if (s is LambdaValue)
                {
                    return null;
                }

                return s.Get(n2.Token.Text.ToString()).Members;
            }
            else if (b.Left is BinaryOperatorNode ib && ib.Operator.Text.Span == ".")
            {
                return GetScope(b.Left, scope);
            }
        }

        return scope;
    }
}
