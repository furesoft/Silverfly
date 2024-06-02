using Furesoft.PrattParser.Parselets.Builder.Elements;

namespace Furesoft.PrattParser.Parselets.Builder;

public static class Helpers
{
    public static SyntaxElement expr(string name = null)
    {
        return new ExprElement(name);
    }
}