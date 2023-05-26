using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser;

public class TranslationUnit<T>
{
    public T Tree { get; }
    public SourceDocument Document { get; }

    public TranslationUnit(T tree, SourceDocument document)
    {
        Tree = tree;
        Document = document;
    }
}
