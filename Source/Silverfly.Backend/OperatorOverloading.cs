using System.Collections.Generic;
using System.Linq;
using DistIL.AsmIO;

namespace Silverfly.Backend;

public static class OperatorOverloading
{
    public static readonly Dictionary<string, (string Name, bool isStatic)> BinMap = new()
    {
        ["+"] = ("op_Addition", true),
        ["/"] = ("op_Division", true),
        ["-"] = ("op_Subtraction", true),
        ["*"] = ("op_Multiply", true),
        ["%"] = ("op_Modulus", true),

        ["&"] = ("op_BitwiseAnd", true),
        ["|"] = ("op_BitwiseOr", true),
        ["^"] = ("op_ExclusiveOr", true),
        ["=="] = ("op_Equality", true),
        ["!="] = ("op_Inequality", true),

        ["+="] = ("op_AdditionAssignment", false),
        ["-="] = ("op_SubtractionAssignment", false),
        ["*="] = ("op_MultiplicationAssignment", false),
        ["/="] = ("op_DivisionAssignment", false),
    };

    public static readonly Dictionary<string, (string Name, bool isStatic)> UnMap = new()
    {
        ["!"] = ("op_LogicalNot", true),
        ["-"] = ("op_UnaryNegation", true),
        ["~"] = ("op_OnesComplement", true),

        ["implicit"] = ("op_Implicit", true),
        ["explicit"] = ("op_Explicit", true),

        // instance operators
        ["++"] = ("op_IncrementAssignment", false),
        ["--"] = ("op_DecrementAssignment", false),
    };

    public static bool TryGetOperator(this TypeDesc type, string op, out MethodDesc? opMethod, params TypeDesc[] args)
    {
        var nameMap = args.Length switch
        {
            1 => UnMap,
            2 => BinMap,
            _ => null
        };

        if (nameMap!.TryGetValue(op, out var opMethodName))
        {
            var candidate = type.FindMethod(opMethodName.Name, new MethodSig(new TypeSig(), args.Select(t => new TypeSig(t)).ToList(), false), throwIfNotFound: false);

            if (candidate != null)
            {
                opMethod = candidate;
                return true;
            }
        }

        opMethod = null;
        return false;
    }
}
