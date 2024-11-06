using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DistIL.AsmIO;
using Silverfly.Nodes;

namespace Silverfly.Backend.TypeSystem;

public class TypeEngine
{
    private readonly List<TypeRule> _rules = new();

    public void AddRule(TypeRule rule) => _rules.Add(rule);

    public TypeDesc InferType(AstNode node)
    {
        foreach (var rule in _rules)
        {
            try
            {
                return rule.InferType(node, this);
            }
            catch (InvalidOperationException)
            {

            }
        }

        throw new InvalidOperationException("Cannot infer type");
    }

    public bool IsCompatible(TypeDesc left, TypeDesc right)
    {
        return _rules.Any(rule => rule.IsCompatible(left, right));
    }
}
