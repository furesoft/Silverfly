using System;
using System.Collections.Generic;
using System.Linq;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser.Parselets.Builder;

public class BuilderParselet<TNode>(int bindingPower, SyntaxElement definition) : IPrefixParselet, IStatementParselet
    where TNode : AstNode
{
    public int GetBindingPower() => bindingPower;

    public AstNode Parse(Parser parser, Token token)
    {
        var result = new List<(string Name, AstNode Node)>();

        definition.CurrentToken = token;
        var r = definition.Parse(parser, result);

        if (!r)
        {
            //ToDo: Add Better Error Message
            token.Document.Messages.Add(Message.Error($"Unable to parse builded pattern: {definition}"));
        }

        var node = InitNode(result);

        return node.WithRange(token, parser.LookAhead(0));
    }

    private TNode InitNode(List<(string Name, AstNode Node)> result)
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

    private static void SetProperties(TNode node, List<(string Name, AstNode Node)> result)
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