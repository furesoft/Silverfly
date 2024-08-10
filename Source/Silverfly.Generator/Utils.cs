using Microsoft.CodeAnalysis;

namespace Silverfly.Generator;

public static class Utils
{
    public static INamedTypeSymbol? InheritsFrom(INamedTypeSymbol classSymbol, string name)
    {
        var baseType = classSymbol.BaseType;
        while (baseType != null)
        {
            if (baseType.Name == name)
                break;
            baseType = baseType.BaseType;
        }

        return baseType;
    }
}
