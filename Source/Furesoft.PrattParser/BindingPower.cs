namespace Furesoft.PrattParser;

/// <summary>
/// Defines the different binding power levels used by the infix parsers. These
/// determine how a series of infix expressions will be grouped. For example,
/// "a + b * c - d" will be parsed as "(a + (b * c)) - d" because "*" has higher
/// binding power than "+" and "-". Here, bigger numbers mean higher binding power.
/// 
/// Binding power implements operator precedence, which is a more common term, but
/// given how Pratt parsing works, I think binding power is more apropos.
/// </summary>
public class BindingPower : Symbol
{
    // Ordered in increasing binding power.
    public static BindingPower Assignment;
    public static BindingPower Conditional;
    public static BindingPower Sum;
    public static BindingPower Product;
    public static BindingPower Exponent;
    public static BindingPower Prefix;
    public static BindingPower PostFix;
    public static BindingPower Call;
    
    public new static SymbolPool<BindingPower> Pool => new(f => new(f));

    internal BindingPower(int id, string name, SymbolPool pool) : base(id, name, pool)
    {
    }

    static BindingPower()
    {
        Assignment = new(1, nameof(Assignment), Pool);
        Conditional = new(2, nameof(Conditional), Pool);
        Sum = new(3, nameof(Sum), Pool);
        Product = new(4, nameof(Product), Pool);
        Exponent = new(5, nameof(Exponent), Pool);
        Prefix = new(6, nameof(Prefix), Pool);
        PostFix = new(7, nameof(PostFix), Pool);
        Call = new(8, nameof(Call), Pool);
    }

    protected BindingPower(Symbol prototype) : base(prototype)
    {
    }
}
