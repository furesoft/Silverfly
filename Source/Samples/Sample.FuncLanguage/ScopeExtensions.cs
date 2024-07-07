using Sample.FuncLanguage.Values;

namespace Sample.FuncLanguage;

public static class ScopeExtensions
{
    public static void DefineOperator(this Scope scope, string name, Func<double, double, double> func)
    {
        scope.Define("'" + name, (Value[] args) =>
        {
            var aV = (NumberValue)args[0];
            var bV = (NumberValue)args[1];

            return new NumberValue(func(aV.Value, bV.Value));
        });
    }
}