namespace Silverfly;

public static class PredefinedSymbols
{
    public static SymbolPool Pool = GSymbol.Pool;

    public static Symbol EOF = Pool.Get("#eof");

    /// <summary>Represents the start of the document</summary>
    public static Symbol SOF = Pool.Get("#sof");

    public static Symbol EOL = Pool.Get("\n");

    public static Symbol String = Pool.Get("#string");
    public static Symbol Number = Pool.Get("#number");
    public static Symbol Boolean = Pool.Get("#boolean");
    public static Symbol Name = Pool.Get("#name");

    public static Symbol SlashSlash = Pool.Get("//");
    public static Symbol SlashAsterisk = Pool.Get("/*");
    public static Symbol AsteriskSlash = Pool.Get("*/");
}
