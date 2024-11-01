using System.Collections.Generic;
using System.Collections.Immutable;
using Silverfly.Nodes;
using Silverfly.Text;

namespace Silverfly;

public partial class Parser
{
    /// <summary>
    /// Parses a list of AST nodes separated by a given separator and terminated by a terminator symbol.
    /// </summary>
    /// <param name="separator">The symbol used to separate the nodes.</param>
    /// <param name="terminator">The symbol used to terminate the list.</param>
    /// <param name="bindingPower">The binding power for parsing. Default is 0.</param>
    /// <returns>An immutable list of AST nodes.</returns>
    public ImmutableList<AstNode> ParseSeperated(Symbol separator, Symbol terminator, int bindingPower = 0)
    {
        var token = LookAhead();
        var args = new List<AstNode>();

        if (Match(terminator))
        {
            return [];
        }

        do
        {
            var node = Parse(bindingPower);

            if (node is not InvalidNode)
            {
                args.Add(node);
            }
        } while (Match(separator) && Lexer.IsNotAtEnd());

        if (!IsMatch(terminator))
        {
            Document.AddMessage(MessageSeverity.Error, $"{terminator} is missing", token.GetRange());
        }

        Consume();

        return [.. args];
    }

    /// <summary>
    /// Parses a list of AST nodes terminated by any of the specified terminators.
    /// </summary>
    /// <param name="bindingPower">The binding power for parsing. Default is 0.</param>
    /// <param name="terminators">The symbols used to terminate the list.</param>
    /// <returns>An immutable list of AST nodes.</returns>
    public ImmutableList<AstNode> ParseList(int bindingPower = 0, params Symbol[] terminators)
    {
        var args = new List<AstNode>();

        while (!IsMatch(terminators) && Lexer.IsNotAtEnd())
        {
            var node = Parse(bindingPower);

            if (node is not InvalidNode)
            {
                args.Add(node);
            }
        }

        Match(terminators);

        return [.. args];
    }

    /// <summary>
    /// Parses a list of AST nodes separated by a given separator and terminated by any of the specified terminators.
    /// </summary>
    /// <param name="separator">The symbol used to separate the nodes.</param>
    /// <param name="bindingPower">The binding power for parsing. Default is 0.</param>
    /// <param name="terminators">The symbols used to terminate the list.</param>
    /// <returns>An immutable list of AST nodes.</returns>
    public ImmutableList<AstNode> ParseSeperated(Symbol separator, int bindingPower = 0, params Symbol[] terminators)
    {
        var args = new List<AstNode>();

        if (Match(terminators))
        {
            return [];
        }

        do
        {
            var node = Parse(bindingPower);

            if (node is not InvalidNode)
            {
                args.Add(node);
            }
        } while (Match(separator) && Lexer.IsNotAtEnd());

        Match(terminators);

        return [.. args];
    }

    public bool IsAtEnd() => IsMatch(PredefinedSymbols.EOF);
}
