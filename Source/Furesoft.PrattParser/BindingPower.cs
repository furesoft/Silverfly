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
public static class BindingPower {
   // Ordered in increasing binding power.
   public static int Assignment = 1;
   public static int Conditional = 2;
   public static int Sum = 3;
   public static int Product = 4;
   public static int Exponent = 5;
   public static int Prefix = 6;
   public static int PostFix = 7;
   public static int Call = 8;
}