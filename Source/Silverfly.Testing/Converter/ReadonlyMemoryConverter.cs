namespace Silverfly.Testing.Converter;

public class ReadonlyMemoryConverter : WriteOnlyJsonConverter<ReadOnlyMemory<char>>
{
    public override void Write(VerifyJsonWriter writer, ReadOnlyMemory<char> value)
    {
        writer.WriteRawValue(value.ToString());
    }
}
