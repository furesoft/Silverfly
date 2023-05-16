using System.Text;

namespace Furesoft.PrattParser.Expressions;

/// <summary>
/// A simple variable name expression like "abc".
/// </summary>
public class NameAstNode : IAstNode
{
    public NameAstNode(string name)
    {
        Name = name;
    }

    public string Name
    {
        get;
    }
}
