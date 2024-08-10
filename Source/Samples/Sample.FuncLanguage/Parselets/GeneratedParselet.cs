using Silverfly.Generator;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

[Parselet("'def' <id:name> '=' <expr:value>", typeof(VariableBindingNode))]
public partial class GeneratedParselet : IPrefixParselet
{
    //public AstNode Parse(Parser parser, Token token) { return null; }
}
