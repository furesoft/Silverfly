namespace Furesoft.PrattParser;

public class PredefinedSymbols
{
    public static SymbolPool Pool = new (0, 0);
    
    public static Symbol EOF = new(0, "\0", Pool);
    
    public static Symbol Comma = new(1, ",", Pool);
    public static Symbol Colon = new(2, ":", Pool);
    public static Symbol Equals = new(3, "=", Pool);
    
    public static Symbol LeftParen = new(4, "(", Pool);
    public static Symbol RightParen = new(5, "(", Pool);
    
    public static Symbol Question = new(6, "?", Pool);
    
    public static Symbol Plus = new(7, "+", Pool);
    public static Symbol Minus = new(8, "-", Pool);
    public static Symbol Asterisk = new(9, "*", Pool);
    public static Symbol Slash = new(10, "/", Pool);
    
    public static Symbol Caret = new(11, "^", Pool);
    public static Symbol Tilde = new(12, "~", Pool);
    public static Symbol Bang = new(13, "!", Pool);
    
    public static Symbol Name = new(14, "#name", Pool);
    
    public static Symbol Pipe = new(15, "|", Pool);
    public static Symbol Semicolon = new(16, ";", Pool);
    public static Symbol Dollar = new(17, "$", Pool);
    public static Symbol Percentage = new(18, "%", Pool);
    
    public static Symbol LeftBracket = new(19, "[", Pool);
    public static Symbol RightBracket = new(20, "]", Pool);
    
    public static Symbol LeftCurly = new(21, "{", Pool);
    public static Symbol RightCurly = new(22, "}", Pool);
    
    public static Symbol Backslash = new(23, "\\", Pool);
    
    public static Symbol String = new(24, "#string", Pool);
}
