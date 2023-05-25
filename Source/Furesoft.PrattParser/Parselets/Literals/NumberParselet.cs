using System;
using System.Globalization;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class NumberParselet : IPrefixParselet<AstNode>
{
    public AstNode Parse(Parser<AstNode> parser, Token token)
    {
        if (token.Text.StartsWith("0x"))
        {
            return new LiteralNode<uint>(uint.Parse(token.Text[2..], NumberStyles.HexNumber));
        }
        
        if (token.Text.StartsWith("0b"))
        {
            return new LiteralNode<uint>(Convert.ToUInt32(token.Text[2..], 2));
        }
        
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
