namespace Silverfly;

public class DefaultPrecedenceLevels : PrecedenceLevels
{
    public static string Assignment = nameof(Assignment);
    public static string Conditional = nameof(Conditional);
    public static string Sum = nameof(Sum);
    public static string Product = nameof(Sum);
    public static string Exponent = nameof(Exponent);
    public static string Prefix = nameof(Prefix);
    public static string PostFix = nameof(PostFix);
    public static string Call = nameof(Call);

    public DefaultPrecedenceLevels()
    {
        AddPrecedence(Assignment);
        AddPrecedence(Conditional);
        AddPrecedence(Sum);
        AddPrecedence(Product);
        AddPrecedence(Exponent);
        AddPrecedence(Prefix);
        AddPrecedence(PostFix);
        AddPrecedence(Call);
    }
}
