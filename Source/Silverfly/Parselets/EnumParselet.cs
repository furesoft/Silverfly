using System;
using Silverfly.Nodes;

namespace Silverfly.Parselets;

public class EnumParselet<TRegister> : IPrefixParselet
    where TRegister : struct
{
    public AstNode Parse(Parser parser, Token token)
    {
        var value = Enum.Parse<TRegister>(token.ToString(), true);

        return new LiteralNode(value, token)
            .WithRange(token);
    }
}
