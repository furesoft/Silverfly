using Silverfly.Nodes;
using Silverfly.Text;

namespace Silverfly;

public class TranslationUnit(AstNode tree, SourceDocument document)
{
    public AstNode Tree { get; } = tree;
    public SourceDocument Document { get; } = document;
}
