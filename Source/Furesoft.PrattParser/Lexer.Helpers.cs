using System;
using Furesoft.PrattParser.Lexing.IgnoreMatcher;
using Furesoft.PrattParser.Lexing.Matcher;

namespace Furesoft.PrattParser;

public partial class Lexer
{
    public void MatchString(Symbol leftSymbol, Symbol rightSymbol, bool allowEscapeChars = true, bool allowUnicodeChars = true)
    {
        AddMatcher(new StringMatcher(leftSymbol, rightSymbol, allowEscapeChars, allowUnicodeChars));
    }

    public void MatchNumber(bool allowHex, bool allowBin, Symbol floatingPointSymbol = null,
        Symbol seperatorSymbol = null)
    {
        AddMatcher(new NumberMatcher(allowHex, allowBin, floatingPointSymbol ?? PredefinedSymbols.Dot,
            seperatorSymbol ?? PredefinedSymbols.Underscore));
    }

    public void MatchBoolean(bool ignoreCasing = false)
    {
        AddMatcher(new BooleanMatcher(ignoreCasing));
    }

    public void Ignore(char c)
    {
        Ignore(new PunctuatorIgnoreMatcher(c.ToString()));
    }

    public void Ignore(Predicate<char> predicate)
    {
        Ignore(new PredicateIgnoreMatcher(predicate));
    }

    public void IgnoreWhitespace()
    {
        Ignore(char.IsWhiteSpace);
    }

    public void Ignore(params char[] chars)
    {
        foreach (var c in chars)
        {
            Ignore(c);
        }
    }

    public void Ignore(string c)
    {
        Ignore(new PunctuatorIgnoreMatcher(c));
    }

    public void Ignore(params string[] symbols)
    {
        foreach (var symbol in symbols)
        {
            Ignore(symbol);
        }
    }

    public void AddSymbols(params string[] symbols)
    {
        foreach (var symbol in symbols)
        {
            AddSymbol(symbol);
        }
    }
}