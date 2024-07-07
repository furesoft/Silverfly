using Silverfly.Text;

namespace Silverfly.Testing.Converter;

public class RangeConverter : WriteOnlyJsonConverter<SourceRange>
{
    public override void Write(VerifyJsonWriter writer, SourceRange value)
    {
        writer.WriteRawValue($"{value.Start}-{value.End}");
    }
}
