namespace Silverfly.Sample.Func.Values;

public record OptionValue : Value
{
    public OptionValue()
    {
        Members.Define("__value", UnitValue.Shared);
        Members.Define("__has_value", (Value[] args) =>
        {
            var value = Members.Get("__value");

            return value != UnitValue.Shared;
        });

        Members.Define("to_string", (Value[] args) =>
        {
            if (!HasValue())
            {
                return "none";
            }

            return $"some({Members.Get("__value")})";
        });
    }

    public static readonly OptionValue None = new();

    public static OptionValue Some(Value value)
    {
        var option = new OptionValue();
        option.Members.Define("__value", value);

        return option;
    }

    public static void AddToScope()
    {
        var memberScope = new Scope();

        memberScope.Define("none", None);
        memberScope.Define("some", v => Some(v));

        Scope.Root.Define("Option", new ModuleValue(memberScope));
    }

    public override bool IsTruthy()
    {
        return HasValue();
    }

    private bool HasValue()
    {
        return Members.Get<LambdaValue>("__has_value").Invoke().IsTruthy();
    }

    public override string ToString()
    {
        if (Members.TryGet<LambdaValue>("to_string", out var func))
        {
            return func.Invoke().ToString();
        }

        return "";
    }
}