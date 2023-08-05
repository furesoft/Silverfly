using Furesoft.PrattParser.Text;
using VerifyTests;

namespace Furesoft.PrattParser.Testing;

public class RangeConverter : WriteOnlyJsonConverter<SourceRange>
{
    public override void Write(VerifyJsonWriter writer, SourceRange value)
    {
        writer.WriteRawValue($"{value.Start}-{value.End}");
    }
}
