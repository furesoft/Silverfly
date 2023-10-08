using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser;

public class TranslationUnit(AstNode tree, SourceDocument document)
{
    public AstNode Tree { get; } = tree;
    public SourceDocument Document { get; } = document;
}
