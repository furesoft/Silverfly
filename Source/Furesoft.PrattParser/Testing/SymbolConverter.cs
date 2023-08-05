using VerifyTests;

namespace Furesoft.PrattParser.Testing;

public class SymbolConverter : WriteOnlyJsonConverter<Symbol>
{
    public override void Write(VerifyJsonWriter writer, Symbol value)
    {
        writer.WriteRawValue($"{value.Name}");
    }
}
