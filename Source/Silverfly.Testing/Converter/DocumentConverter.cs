using Silverfly.Text;

namespace Silverfly.Testing.Converter;

public class DocumentConverter : WriteOnlyJsonConverter<SourceDocument>
{
    public override void Write(VerifyJsonWriter writer, SourceDocument value)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Filename");
        writer.WriteRawValue(value.Filename);

        writer.WritePropertyName("Source");
        writer.WriteRawValue(value.Source.ToString());

        writer.WritePropertyName("Messages");
        writer.WriteStartArray();

        foreach (var msg in value.Messages)
        {
            writer.WriteRawValue(msg.ToString());
        }

        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
