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
public enum BindingPower
{
    // Ordered in increasing binding power.
    Assignment = 1,
    Conditional = 2,
    Sum = 3,
    Product = 4,
    Exponent = 5,
    Prefix = 6,
    PostFix = 7,
    Call = 8
}
