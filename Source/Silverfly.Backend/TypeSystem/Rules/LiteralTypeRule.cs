using System;
using DistIL.AsmIO;
using Silverfly.Nodes;

namespace Silverfly.Backend.TypeSystem.Rules;


public class LiteralTypeRule : TypeRule
{
    public override bool IsCompatible(TypeDesc left, TypeDesc right)
    {
        return true; //ToDo: implement
    }

    public override TypeDesc InferType(AstNode node, TypeEngine engine)
    {
        if (node is not LiteralNode literal)
        {
            throw new InvalidOperationException("Incompatible type");
        }

        return literal.Value switch
        {
            bool => PrimType.Bool,
            char => PrimType.Char,
            int => PrimType.Int32,
            double => PrimType.Double,
            string => PrimType.String,
            _ => throw new InvalidOperationException("Incompatible type")
        };
    }
}
