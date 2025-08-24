using System.Reflection;
using DistIL.AsmIO;
using DistIL.IR;

namespace Silverfly.Backend.Scoping.Items;

public class ParameterScopeItem : ScopeItem
{
    public required Argument Arg { get; init; }

    public override TypeDesc Type => Arg.ResultType;

    public bool IsOut => Parameter.Attribs.HasFlag(ParameterAttributes.Out);
    public required ParamDef Parameter { get; init; }


    public void Deconstruct(out string name, out Argument parameter, out TypeDesc type, out bool isOut)
    {
        name = Name;
        parameter = Arg;
        type = Type;
        isOut = IsOut;
    }
}