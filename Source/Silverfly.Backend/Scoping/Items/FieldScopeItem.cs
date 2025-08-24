using System.Reflection;
using DistIL.AsmIO;

namespace Silverfly.Backend.Scoping.Items;

public class FieldScopeItem : ScopeItem
{
    public FieldDesc Field { get; init; }
    public bool IsMutable => !Field.Attribs.HasFlag(FieldAttributes.InitOnly);
    public bool IsStatic => Field.IsStatic;

    public override TypeDesc Type => Field.Type;

    public void Deconstruct(out string name, out FieldDesc field, out bool isMutable, out bool isStatic)
    {
        name = Name;
        field = Field;
        isMutable = IsMutable;
        isStatic = IsStatic;
    }
}