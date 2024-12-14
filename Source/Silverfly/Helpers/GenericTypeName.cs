using System.Collections.Immutable;

namespace Silverfly.Helpers;

public class GenericTypeName : TypeName
{
    public GenericTypeName(Token token, ImmutableList<TypeName> genericArguments) : base(token)
    {
        Properties.Set(nameof(GenericArguments), genericArguments);
    }

    public ImmutableList<TypeName> GenericArguments => Properties.GetOrThrow<ImmutableList<TypeName>>(nameof(GenericArguments));
}
