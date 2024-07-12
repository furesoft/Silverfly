using Sample.FuncLanguage.Nodes;
using Silverfly;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Sample.FuncLanguage.Parselets;

public class ModuleParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var arg = parser.ParseExpression();

        if (arg is NameNode name)
        {
            return new ModuleNode(name.Name);
        }

        return new InvalidNode(token);
    }
}
