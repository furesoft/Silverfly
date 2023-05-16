namespace Furesoft.PrattParser;

public static class PredefinedSymbols
{
    public static SymbolPool Pool = GSymbol.Pool;

    public static Symbol EOF = Pool.Get("#eof");
    
    public static Symbol LeftParen = Pool.Get("(");
    public static Symbol RightParen = Pool.Get(")");
    
    public static Symbol LeftSquare = Pool.Get("[");
    public static Symbol RightSquare = Pool.Get("]");
    
    public static Symbol LessThan = Pool.Get("<");
    public static Symbol GreaterThan = Pool.Get(">");
    
    public static Symbol LessThanLessThan = Pool.Get("<<");
    public static Symbol GreaterThanGreaterThan = Pool.Get(">>");

    public static Symbol Question = Pool.Get("?");

    public static Symbol Ampersand = Pool.Get("&");
    
    public static Symbol At = Pool.Get("@");

    public static Symbol Plus = Pool.Get("+");
    public static Symbol Minus = Pool.Get("-");
    public static Symbol Asterisk = Pool.Get("*");
    public static Symbol Slash = Pool.Get("/");

    public static Symbol Caret = Pool.Get("^");
    public static Symbol Tilde = Pool.Get("~");
    public static Symbol Bang = Pool.Get("!");
    public static Symbol Dot = Pool.Get(".");
    
    public static Symbol Underscore = Pool.Get("_");

    public static Symbol DoubleQuote = Pool.Get("\"");
    public static Symbol SingleQuote = Pool.Get("'");
    
    public static Symbol Comma = Pool.Get(",");
    public static Symbol Colon = Pool.Get(":");
    public new static Symbol Equals = Pool.Get("=");

    public static Symbol Pipe = Pool.Get("|");
    public static Symbol Semicolon = Pool.Get(";");
    public static Symbol Dollar = Pool.Get("$");
    public static Symbol Percentage = Pool.Get("%");

    public static Symbol LeftCurly = Pool.Get("{");
    public static Symbol RightCurly = Pool.Get("}");

    public static Symbol Backslash = Pool.Get("\\");

    public static Symbol Section = Pool.Get("§");
    public static Symbol Degree = Pool.Get("°");

    public static Symbol String = Pool.Get("#string");
    public static Symbol Integer = Pool.Get("#integer");
    public static Symbol Name = Pool.Get("#name");
    
    
    public static Symbol AmpersandAmpersand = Pool.Get("&&");
    public static Symbol PipePipe = Pool.Get("||");
    public static Symbol EqualsEquals = Pool.Get("==");
    public static Symbol DotDot = Pool.Get("..");
    public static Symbol QuestionQuestion = Pool.Get("??");
    public static Symbol QuestionDot = Pool.Get("?.");
    public static Symbol ColonQuestion = Pool.Get(":?");
    
    public static Symbol Arrow = Pool.Get("->");
    public static Symbol DoubleArrow = Pool.Get("=>");
}
