using Furesoft.PrattParser.Parselets.Builder.Elements;

namespace Furesoft.PrattParser.Parselets.Builder;

public static class Helpers
{
    public static SyntaxElement expr()
    {
        return new ExprElement();
    }
}