using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Parselets.Literals;

namespace Furesoft.PrattParser;

public static class ParserExtensions
{
    public static void AddArithmeticOperators(this Parser<AstNode> parser)
    {
        parser.Prefix("+", (int)BindingPower.Prefix);
        parser.Prefix("-", (int)BindingPower.Prefix);
        
        parser.Group("(", ")");
        
        parser.InfixLeft("+", (int)BindingPower.Sum);
        parser.InfixLeft("-", (int)BindingPower.Sum);
        parser.InfixLeft("*", (int)BindingPower.Product);
        parser.InfixLeft("/", (int)BindingPower.Product);
    }
    
    public static void AddLogicalOperators(this Parser<AstNode> parser)
    {
        parser.Prefix("!", (int)BindingPower.Prefix);
        
        parser.InfixLeft("&&", (int)BindingPower.Product);
        parser.InfixLeft("||", (int)BindingPower.Sum);
    }
    
    public static void AddBitOperators(this Parser<AstNode> parser)
    {
        parser.Prefix("~", (int)BindingPower.Prefix);
        
        parser.InfixLeft("&", (int)BindingPower.Product);
        parser.InfixLeft("|", (int)BindingPower.Sum);
        
        parser.InfixLeft("<<", (int)BindingPower.Product);
        parser.InfixLeft(">>", (int)BindingPower.Product);
    }

    public static void AddCommonLiterals(this Parser<AstNode> parser)
    {
        parser.Register(PredefinedSymbols.Integer, new IntegerLiteralParselet());
        parser.Register(PredefinedSymbols.Boolean, new BooleanLiteralParselet());
    }

    public static void AddCommonAssignmentOperators(this Parser<AstNode> parser)
    {
        parser.Register("=", new AssignParselet());
        parser.Register("+=", new AssignParselet());
        parser.Register("-=", new AssignParselet());
        parser.Register("*=", new AssignParselet());
        parser.Register("/=", new AssignParselet());
        
        parser.Prefix("++", (int)BindingPower.Prefix);
        parser.Prefix("--", (int)BindingPower.Prefix);
        
        parser.Postfix("--", (int)BindingPower.PostFix);
        parser.Postfix("++", (int)BindingPower.PostFix);
    }
}
