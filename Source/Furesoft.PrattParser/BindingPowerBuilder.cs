using System;
using System.Collections.Generic;

namespace Furesoft.PrattParser;

public abstract partial class Parser
{
    public class BindingPowerBuilder
    {
        private readonly Dictionary<string, int> _bindingPowers = [];

        public BindingPowerBuilder StrongerThan(params string[] operators)
        {
            var maxBindingPower = 0;
            foreach (string op in operators)
            {
                if (_bindingPowers.TryGetValue(op, out int bindingPower))
                {
                    maxBindingPower = Math.Max(maxBindingPower, bindingPower);
                }
            }

            maxBindingPower++;

            foreach (string op in operators)
            {
                _bindingPowers[op] = maxBindingPower;
            }

            return this;
        }

        internal SymbolPool BuildPool()
        {
            var pool = new SymbolPool();

            foreach (var (operatorName, bindingPower) in _bindingPowers)
            {
                _ = new Symbol(bindingPower, operatorName, pool);
            }

            return pool;
        }
    }
}
