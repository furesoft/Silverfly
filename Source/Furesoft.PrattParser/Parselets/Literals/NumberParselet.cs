using System.Globalization;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class NumberParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        if (!token.Text.StartsWith("-") && !token.Text.Contains("."))
        {
            return new LiteralNode<uint>(uint.Parse(token.Text)).WithRange(token);
        }

        if (token.Text.Contains('.'))
        {
            return new LiteralNode<double>(double.Parse(token.Text, CultureInfo.InvariantCulture)).WithRange(token);
        }
        
        return new LiteralNode<int>(int.Parse(token.Text)).WithRange(token);
    }
}
