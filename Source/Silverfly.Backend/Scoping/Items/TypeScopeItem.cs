using DistIL.AsmIO;

namespace Silverfly.Backend.Scoping.Items;

public class TypeScopeItem : ScopeItem
{
    public TypeDef TypeInfo { get; init; }
    public Scope SubScope { get; init; }

    public override TypeDesc Type => TypeInfo;

    public void Deconstruct(out string name, out TypeDesc type, out Scope subScope)
    {
        name = Name;
        type = Type;
        subScope = SubScope;
    }
}