#nullable enable
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Silverfly.Helpers;

public static class TypeNameParser
{
    public static bool TryParse(Parser parser, out TypeName? typename)
    {
        if (!parser.IsMatch(PredefinedSymbols.Name))
        {
            typename = null;
            return false;
        }

        var name = parser.Consume();

        if (parser.Lexer.Peek() == '<')
        {
            parser.Consume('<');
            var genericArgs = ParseGenericArguments(parser);
            parser.Consume('>');

            typename = (TypeName)new GenericTypeName(name, genericArgs)
                .WithRange(name, parser.LookAhead(0));
            return true;
        }

        typename = (TypeName)new TypeName(name).WithRange(name);
        return true;
    }

    private static ImmutableList<TypeName> ParseGenericArguments(Parser parser)
    {
        var args = new List<TypeName>();

        do
        {
            if (TryParse(parser, out var typename))
            {
                args.Add(typename!);
            }

            if (parser.Lexer.Peek() == ',')
                parser.Consume(',');
        } while (parser.Lexer.Peek() != '>' && parser.Lexer.IsNotAtEnd());

        return args.ToImmutableList();
    }

    public static TypeName? ParseTypeName(this Parser parser)
    {
        return TryParse(parser, out var typename) ? typename : null;
    }
}
