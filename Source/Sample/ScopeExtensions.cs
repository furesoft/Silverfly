namespace Sample;

public static class ScopeExtensions
{
    public static void DefineOperator(this Scope scope, string name, Func<double, double, double> func)
    {
        scope.Define("'" + name, (a, b) =>
        {
            var aV = (NumberValue)a;
            var bV = (NumberValue)b;

            return new NumberValue(func(aV.Value, bV.Value));
        });
    }
}