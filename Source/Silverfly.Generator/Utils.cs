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

    public static bool ImplementsInterface(INamedTypeSymbol classSymbol, string interfaceName)
    {
        // Überprüfe die direkt implementierten Interfaces
        foreach (var @interface in classSymbol.Interfaces)
        {
            if (@interface.Name == interfaceName || @interface.ToDisplayString() == interfaceName)
            {
                return true;
            }
        }

        // Überprüfe rekursiv die Basisklassen
        if (classSymbol.BaseType != null)
        {
            return ImplementsInterface(classSymbol.BaseType, interfaceName);
        }

        return false;
    }
}
