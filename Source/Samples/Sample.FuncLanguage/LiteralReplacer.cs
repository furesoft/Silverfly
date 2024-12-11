using MrKWatkins.Ast.Traversal;
using Silverfly.Nodes;

namespace Silverfly.Sample.Func;

public partial class LiteralReplacer : MrKWatkins.Ast.Processing.Replacer<AstNode, LiteralNode>
{
    protected override ITraversal<AstNode> Traversal => DepthFirstPreOrderTraversal<AstNode>.Instance;
    protected override bool ShouldProcessChildren(AstNode node)
    {
        return true;
    }

    protected override AstNode ReplaceNode(LiteralNode node)
    {
        if (node.Value is ulong value)
        {
            return new LiteralNode((double)value, node.Token);
        }

        return node;
    }
}
