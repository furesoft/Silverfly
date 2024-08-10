using Silverfly.Generator;
using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Sample.Func.Nodes;

namespace Silverfly.Sample.Func.Parselets;

[Parselet("'def' <id:name> '=' <expr:value>", typeof(VariableBindingNode))]
public partial class GeneratedParselet : IPrefixParselet
{
}
