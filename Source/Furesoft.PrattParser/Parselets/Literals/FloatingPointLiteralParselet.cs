using System.Globalization;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class FloatingPointLiteralParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        return new LiteralNode<double>(double.Parse(token.Text, CultureInfo.InvariantCulture)).WithRange(token);
    }
}
