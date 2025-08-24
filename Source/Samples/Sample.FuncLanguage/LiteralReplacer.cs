using MrKWatkins.Ast.Processing;
using MrKWatkins.Ast.Traversal;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func;

public class LiteralReplacer : NodeReplacer<AstNode, LiteralNode>
{
    protected override bool ShouldProcessDescendents(LiteralNode node) => false;

    protected override LiteralNode Replace(LiteralNode node)
    {
        if (node.Value is ulong value)
        {
            return new LiteralNode((double)value, node.Token);
        }

        return node;
    }
}
