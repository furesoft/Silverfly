using System;

namespace Silverfly.Lexing.IgnoreMatcher;

public class PredicateIgnoreMatcher(Predicate<char> predicate) : IIgnoreMatcher
{
    public bool Match(Lexer lexer, char c)
    {
        return predicate(c);
    }

    public void Advance(Lexer lexer)
    {
        lexer.Advance(1);
    }
}
