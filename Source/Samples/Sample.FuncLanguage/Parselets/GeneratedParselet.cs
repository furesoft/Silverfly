using Silverfly.Generator;
using Silverfly.Nodes;
using Silverfly.Parselets;

namespace Silverfly.Sample.Func.Parselets;

// _ ignore token
// : name a token
//<> nonterminal like <expr>, <block>, <statement>
// ? 0..1
// + 1..n
// * 0..n
[Parselet("_'let' 'mut'? <id:Name> '=' <expr:Value>", typeof(DefNode))]
public partial class GeneratedParselet : IPrefixParselet
{
}

public record DefNode(Token Name, AstNode Value) : AstNode;
