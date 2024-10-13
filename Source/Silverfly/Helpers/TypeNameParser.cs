#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Silverfly.Helpers;

public class TypeNameParser
{
    public Symbol Start { get; set; }
    public Symbol End { get; set; }
    public Symbol Seperator { get; set; }

    public bool TryParse(Parser parser, out TypeName? typename)
    {
        if (!parser.IsMatch(PredefinedSymbols.Name))
        {
            typename = null;
            return false;
        }

        using var context = parser.Lexer.OpenContext<TypenameContext>();
        var name = parser.Consume();

        if (parser.LookAhead().Type == Start)
        {
            parser.Consume(Start);
            var genericArgs = ParseGenericArguments(parser);
            parser.Consume(End);

            typename = (TypeName)new GenericTypeName(name, genericArgs)
                .WithRange(name, parser.LookAhead(0));
            return true;
        }

        typename = (TypeName)new TypeName(name).WithRange(name);
        return true;
    }

    private ImmutableList<TypeName> ParseGenericArguments(Parser parser)
    {
        var args = new List<TypeName>();

        do
        {
            if (TryParse(parser, out var typename))
            {
                args.Add(typename!);
            }

            if (parser.LookAhead().Type == Seperator)
                parser.Consume(Seperator);
        } while (parser.LookAhead().Type != End && parser.Lexer.IsNotAtEnd());

        return args.ToImmutableList();
    }
}
