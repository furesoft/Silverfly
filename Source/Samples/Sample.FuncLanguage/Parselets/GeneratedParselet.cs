using Silverfly.Generator;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

// _ ignore token
// : name a token
//<> nonterminal like <expr>, <block>, <statement>
[Parselet("_'def' <id:name> '=' <expr:value>", typeof(VariableBindingNode))]
public partial class GeneratedParselet : IPrefixParselet
{
}
