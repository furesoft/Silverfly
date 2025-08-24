using DistIL.AsmIO;

namespace Silverfly.Backend.Scoping;

public abstract class ScopeItem
{
    public required string Name { get; init; }

    public bool IsMutable { get; init; }
    public abstract TypeDesc Type { get; }

    public object UserData { get; set; }
}
