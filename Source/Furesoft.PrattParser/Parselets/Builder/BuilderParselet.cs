using System;
using System.Collections.Generic;
using System.Linq;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Builder;

public class BuilderParselet<TNode>(int bindingPower, SyntaxElement definition) : IPrefixParselet, IStatementParselet
    where TNode : AstNode
{
    public int GetBindingPower() => bindingPower;

    public AstNode Parse(Parser parser, Token token)
    {
        var parsedNodes = new List<(string, object)>();

        definition.CurrentToken = token;
        var parsedSuccessfull = definition.Parse(parser, parsedNodes);

        if (!parsedSuccessfull)
        {
            token.Document.Messages.AddRange(definition.Messages);
        }

        var node = CreateNode(parsedNodes);

        return node.WithRange(token, parser.LookAhead(0));
    }

    private static TNode CreateNode(List<(string Name, object Node)> result)
    {
        TNode node;
        try
        {
            node = (TNode)Activator.CreateInstance(typeof(TNode),
            [.. result.Select(_ => _.Node)]);
        }
        catch (MissingMethodException)
        {
            node = Activator.CreateInstance<TNode>();

            SetProperties(node, result);
        }

        return node;
    }

    private static void SetProperties(TNode node, List<(string, object)> result)
    {
        var nodeType = node.GetType();

        foreach (var (name, value) in result)
        {
            var prop = nodeType.GetProperty(name) ??
                throw new MissingMemberException($"{name} is not a property of {nodeType.FullName}");

            prop.SetValue(node, value);
        }
    }
}