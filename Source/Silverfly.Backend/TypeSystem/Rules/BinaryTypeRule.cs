using System;
using DistIL.AsmIO;
using Silverfly.Nodes;
using Silverfly.Nodes.Operators;

namespace Silverfly.Backend.TypeSystem.Rules;

public class BinaryTypeRule : TypeRule
{
    public override bool IsCompatible(TypeDesc left, TypeDesc right)
    {
        return true;
    }

    public override TypeDesc InferType(AstNode node, TypeEngine engine)
    {
        if (node is not BinaryOperatorNode binary)
        {
            throw new InvalidOperationException("Incompatible type");
        }

        var inferredLeft = engine.InferType(binary.Left);
        var inferredRight = engine.InferType(binary.Right);

        if (inferredLeft == inferredRight)
        {
            return inferredLeft;
        }
        
        return TypeDesc.GetCommonAncestor(inferredLeft, inferredRight);
    }
}
