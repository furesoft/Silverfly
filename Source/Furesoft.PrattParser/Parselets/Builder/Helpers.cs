using Furesoft.PrattParser.Parselets.Builder.Elements;

namespace Furesoft.PrattParser.Parselets.Builder;

public static class Helpers
{
    public static SyntaxElement expr(string name = null)
    {
        return new ExprElement(name);
    }

    public static SyntaxElement stmt(string name = null)
    {
        return new StmtElement(name);
    }

    public static SyntaxElement sepBy(Symbol seperator, Symbol terminator, string name = null) {
        return new SepByElement(name, seperator, terminator);
    }
}