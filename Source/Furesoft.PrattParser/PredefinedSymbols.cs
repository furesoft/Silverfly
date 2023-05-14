namespace Furesoft.PrattParser;

public static class PredefinedSymbols
{
    public static SymbolPool Pool = GSymbol.Pool;
    
    public static Symbol EOF = Pool.Get("\0");
    
    public static Symbol Comma = Pool.Get(",");
    public static Symbol Colon = Pool.Get(":");
    public static Symbol Equals = Pool.Get("=");
    
    public static Symbol LeftParen = Pool.Get("(");
    public static Symbol RightParen = Pool.Get(")");
    
    public static Symbol Question = Pool.Get("?");
    
    public static Symbol Ampersand = Pool.Get("&");
    public static Symbol AmpersandAmpersand = Pool.Get("&&");
    public static Symbol PipePipe = Pool.Get("||");
    public static Symbol EqualsEquals = Pool.Get("==");
    
    public static Symbol Arrow = Pool.Get("->");
    public static Symbol DoubleArrow = Pool.Get("=>");
    
    public static Symbol At = Pool.Get("@");
    
    public static Symbol Plus = Pool.Get("+");
    public static Symbol Minus = Pool.Get("-");
    public static Symbol Asterisk = Pool.Get("*");
    public static Symbol Slash = Pool.Get("/");
    
    public static Symbol Caret = Pool.Get("^");
    public static Symbol Tilde = Pool.Get("~");
    public static Symbol Bang = Pool.Get("!");
    
    public static Symbol Name = Pool.Get("#name");
    
    public static Symbol Pipe = Pool.Get("|");
    public static Symbol Semicolon = Pool.Get(";");
    public static Symbol Dollar = Pool.Get("$");
    public static Symbol Percentage = Pool.Get("%");
    
    public static Symbol LeftBracket = Pool.Get("<");
    public static Symbol RightBracket = Pool.Get(">");
    
    public static Symbol LeftSquare = Pool.Get("[");
    public static Symbol RightSquare = Pool.Get("]");
    
    public static Symbol LeftCurly = Pool.Get("{");
    public static Symbol RightCurly = Pool.Get("}");
    
    public static Symbol Backslash = Pool.Get("\\");
    
    public static Symbol String = Pool.Get("#string");
}
