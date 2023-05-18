using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Parselets.Literals;

namespace Furesoft.PrattParser;

public static class ParserExtensions
{
    public static void AddArithmeticOperators<T>(this Parser<T> parser)
    {
        parser.Prefix("+", (int)BindingPower.Prefix);
        parser.Prefix("-", (int)BindingPower.Prefix);
        
        parser.Group("(", ")");
        
        parser.InfixLeft("+", (int)BindingPower.Sum);
        parser.InfixLeft("-", (int)BindingPower.Sum);
        parser.InfixLeft("*", (int)BindingPower.Product);
        parser.InfixLeft("/", (int)BindingPower.Product);
    }
    
    public static void AddLogicalOperators<T>(this Parser<T> parser)
    {
        parser.Prefix("!", (int)BindingPower.Prefix);
        
        parser.InfixLeft("&&", (int)BindingPower.Product);
        parser.InfixLeft("||", (int)BindingPower.Sum);
    }
    
    public static void AddBitOperators<T>(this Parser<T> parser)
    {
        parser.Prefix("~", (int)BindingPower.Prefix);
        
        parser.InfixLeft("&", (int)BindingPower.Product);
        parser.InfixLeft("|", (int)BindingPower.Sum);
        
        parser.InfixLeft("<<", (int)BindingPower.Product);
        parser.InfixLeft(">>", (int)BindingPower.Product);
    }

    public static void AddCommonLiterals<T>(this Parser<T> parser)
    {
        parser.Register(PredefinedSymbols.Integer, (IInfixParselet<T>)new IntegerLiteralParselet());
        parser.Register(PredefinedSymbols.Boolean, (IInfixParselet<T>)new BooleanLiteralParselet());
    }
}
