using Silverfly.Nodes;
using Silverfly.Text;

namespace Silverfly;

/// <summary>
/// Represents a translation unit consisting of an abstract syntax tree (AST) and its associated source document.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TranslationUnit"/> class.
/// </remarks>
/// <param name="tree">The abstract syntax tree (AST) representing the translation unit.</param>
/// <param name="document">The source document associated with the translation unit.</param>
public class TranslationUnit(AstNode tree, SourceDocument document)
{
    /// <summary>
    /// Gets the abstract syntax tree (AST) of the translation unit.
    /// </summary>
    public AstNode Tree { get; } = tree;

    /// <summary>
    /// Gets the source document associated with the translation unit.
    /// </summary>
    public SourceDocument Document { get; } = document;
}
