using Silverfly.Generator;
using Silverfly.Parselets;

namespace Silverfly.Sample.Func.Parselets;

[Parselet("_def <id> '=' <expr>")]
public partial class GeneratedParselet : IPrefixParselet
{
    
}
