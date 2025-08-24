using System.Collections.Generic;
using System.Linq;
using DistIL.AsmIO;

namespace Silverfly.Backend;

public static class OperatorOverloading
{
    public static readonly Dictionary<string, string> BinMap = new()
    {
        ["+"] = "op_Addition",
        ["/"] = "op_Division",
        ["-"] = "op_Subtraction",
        ["*"] = "op_Multiply",
        ["%"] = "op_Modulus",

        ["&"] = "op_BitwiseAnd",
        ["|"] = "op_BitwiseOr",
        ["^"] = "op_ExclusiveOr",
        ["=="] = "op_Equality",
        ["!="] = "op_Inequality",
    };

    public static readonly Dictionary<string, string> UnMap = new()
    {
        ["!"] = "op_LogicalNot",
        ["-"] = "op_UnaryNegation",
        ["~"] = "op_OnesComplement",

        ["implicit"] = "op_Implicit",
        ["explicit"] = "op_Explicit",
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
            var candidate = type.FindMethod(opMethodName, new MethodSig(new TypeSig(), args.Select(t => new TypeSig(t)).ToList(), false), throwIfNotFound: false);

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
