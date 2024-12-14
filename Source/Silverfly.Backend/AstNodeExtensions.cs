﻿using System;
using DistIL.IR;
using Silverfly.Nodes;

namespace Silverfly.Backend;

public static class AstNodeExtensions
{
    public static Value? ToConstant(this AstNode astNode)
    {
        if (astNode is LiteralNode literalNode)
        {
            return literalNode.Value switch
            {
                bool b => ConstInt.CreateI(b ? 1 : 0),
                char ch => ConstInt.CreateI(ch),
                byte b => ConstInt.CreateI(b),
                short s => ConstInt.CreateI(s),
                int i => ConstInt.CreateI(i),
                long l => ConstInt.CreateL(l),

                sbyte sb => ConstInt.CreateI(sb),
                ushort us => ConstInt.CreateI(us),

                float f => ConstFloat.CreateS(f),
                double d => ConstFloat.CreateD(d),

                string s => ConstString.Create(s),

                null => ConstNull.Create(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return null;
    }


    public static BinaryOp MapBinOperator(string op) =>
        op switch
        {
            "+" => BinaryOp.Add,
            "-" => BinaryOp.Sub,
            "*" => BinaryOp.Mul,
            "/" => BinaryOp.FDiv,
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
}
