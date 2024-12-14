using DistIL.AsmIO;
using Silverfly.Nodes;

namespace Silverfly.Backend.TypeSystem;

public abstract class TypeRule
{
    public abstract bool IsCompatible(TypeDesc left, TypeDesc right);

    public abstract TypeDesc InferType(AstNode node, TypeEngine engine);
}
