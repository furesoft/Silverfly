using Furesoft.PrattParser.Text;
using VerifyTests;

namespace Furesoft.PrattParser.Testing.Converter;

public class DocumentConverter : WriteOnlyJsonConverter<SourceDocument>
{
    public override void Write(VerifyJsonWriter writer, SourceDocument value)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Filename");
        writer.WriteRawValue(value.Filename);

        writer.WritePropertyName("Source");
        writer.WriteRawValue(value.Source.ToString());

        writer.WriteEndObject();
    }
}
