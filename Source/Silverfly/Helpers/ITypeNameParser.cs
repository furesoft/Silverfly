#nullable enable
namespace Silverfly.Helpers;

public interface ITypeNameParser
{
    bool TryParse(Parser parser, out TypeName? typename);
}
