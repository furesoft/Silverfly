using VerifyTests;

namespace Silverfly.Testing.Converter;

public class SymbolConverter : WriteOnlyJsonConverter<Symbol>
{
    public override void Write(VerifyJsonWriter writer, Symbol value)
    {
        writer.WriteRawValue($"{value.Name}");
    }
}
