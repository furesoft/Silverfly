using System;
using System.Text;

namespace Silverfly.Helpers;

public record PointerTypeName(Token Token, TypeName Type, PointerKind Kind) : TypeName(Token)
{
    public override string ToString()
    {
        var builder = new StringBuilder();

        switch (Kind)
        {
            case PointerKind.Transient:
                builder.Append('*');
                break;
            case PointerKind.Reference:
                builder.Append('&');
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        builder.Append(Type);

        return builder.ToString();
    }
}
