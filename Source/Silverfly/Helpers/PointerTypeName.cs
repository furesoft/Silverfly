using System;
using System.Text;

namespace Silverfly.Helpers;

public class PointerTypeName : TypeName
{
    public PointerTypeName(Token token, TypeName type, PointerKind kind) : base(token)
    {
        Properties.Set(nameof(Type), type);
        Properties.Set(nameof(Kind), kind);
    }

    public TypeName Type => Properties.GetOrThrow<TypeName>(nameof(Type));
    public PointerKind Kind => Properties.GetOrThrow<PointerKind>(nameof(Kind));

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
