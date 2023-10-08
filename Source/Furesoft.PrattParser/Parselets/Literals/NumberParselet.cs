using System;
using System.Globalization;
using Furesoft.PrattParser.Nodes;

namespace Furesoft.PrattParser.Parselets.Literals;

public class NumberParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        var text = token.Text.ToString();
        
        if (text.StartsWith("0x"))
        {
            return new LiteralNode<ulong>(uint.Parse(token.Text.Slice(2).Span, NumberStyles.HexNumber));
        }
        
        if (text.StartsWith("0b"))
        {
            return new LiteralNode<ulong>(Convert.ToUInt32(token.Text.Slice(2).ToString(), 2));
        }
        
        if (!text.StartsWith("-") && !text.Contains("."))
        {
            return new LiteralNode<ulong>(ulong.Parse(token.Text.Span)).WithRange(token);
        }

        if (text.Contains('.'))
        {
            return new LiteralNode<double>(double.Parse(text, CultureInfo.InvariantCulture)).WithRange(token);
        }
        
        return new LiteralNode<long>(long.Parse(token.Text.Span)).WithRange(token);
    }
}
