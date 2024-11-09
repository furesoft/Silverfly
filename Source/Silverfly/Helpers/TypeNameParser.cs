#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Silverfly.Helpers;

public class TypeNameParser
{
    public Symbol? Start { get; set; }
    public Symbol? End { get; set; }
    public Symbol? Separator { get; set; }

    public bool TryParse(Parser parser, out TypeName? typename)
    {
        if (TryParsePointerName(parser, out typename))
        {
            return true;
        }

        if (!parser.IsMatch(PredefinedSymbols.Name))
        {
            typename = null;
            return false;
        }

        using var context = parser.Lexer.OpenContext<TypenameContext>();
        var name = parser.Consume();

        if (Start is null || End is null || Separator is null)
        {
            typename = (TypeName)new TypeName(name).WithRange(name, parser.LookAhead());
            return true;
        }

        if (parser.LookAhead().Type == Start)
        {
            parser.Consume(Start);
            var genericArgs = ParseGenericArguments(parser);
            parser.Consume(End);

            typename = (TypeName)new GenericTypeName(name, genericArgs)
                .WithRange(name, parser.LookAhead());
            return true;
        }

        typename = (TypeName)new TypeName(name).WithRange(name);
        return true;
    }

    private bool TryParsePointerName(Parser parser, out TypeName? pointerTypeName)
    {
        if (!parser.IsMatch("*") && !parser.IsMatch("&"))
        {
            pointerTypeName = null;
            return false;
        }

        var pointerToken = parser.Consume();
        var pointerKind = pointerToken.Type.Name is "*" ? PointerKind.Transient : PointerKind.Reference;

        if (TryParse(parser, out var innerType) && innerType != null)
        {
            pointerTypeName = new PointerTypeName(pointerToken, innerType, pointerKind);

            return true;
        }

        pointerTypeName = null;
        return false;
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

            if (parser.LookAhead().Type == Separator)
                parser.Consume(Separator);
        } while (parser.LookAhead().Type != End && parser.Lexer.IsNotAtEnd());

        return args.ToImmutableList();
    }
}
