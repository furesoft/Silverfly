using System.Collections.Immutable;

namespace Silverfly.Helpers;

public record GenericTypeName(Token Token, ImmutableList<TypeName> GenericArguments) : TypeName(Token)
{
}
