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
    public static readonly BindingPower Assignment = new(1, nameof(Assignment), Pool);
    public static readonly BindingPower Conditional = new(2, nameof(Conditional), Pool);
    public static readonly BindingPower Sum = new(3, nameof(Sum), Pool);
    public static readonly BindingPower Product = new(4, nameof(Product), Pool);
    public static readonly BindingPower Exponent = new(5, nameof(Exponent), Pool);
    public static readonly BindingPower Prefix = new(6, nameof(Prefix), Pool);
    public static readonly BindingPower PostFix = new(7, nameof(PostFix), Pool);
    public static readonly BindingPower Call = new(8, nameof(Call), Pool);
    
    public new static SymbolPool<BindingPower> Pool => new(f => new(f));

    internal BindingPower(int id, string name, SymbolPool pool) : base(id, name, pool)
    {
    }

    protected BindingPower(Symbol prototype) : base(prototype)
    {
    }
}
