using System;
using System.Collections.Generic;
using System.Linq;
using Furesoft.PrattParser.IgnoreMatcher;
using Furesoft.PrattParser.Matcher;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser;

public sealed class Lexer
{
    private Dictionary<string, Symbol> _punctuators = new();
    private readonly List<ILexerMatcher> _parts = new();
    private readonly List<ILexerIgnoreMatcher> _ignoreMatcher = new();
    private int _index;
    private int _line = 1, _column = 1;

    public SourceDocument Document { get; }

    /// <summary>
    /// Creates a new <see cref="Lexer"/> to tokenize the given string.
    /// </summary>
    /// <param name="source">String to tokenize</param>
    public Lexer(string source, string filename = "tmp.synthetic")
    {
        _index = 0;
        Document = new() {Filename = filename, Source = source};

        // Register all of the Symbols that are explicit punctuators.
        foreach (var type in PredefinedSymbols.Pool)
        {
            var punctuator = type.Punctuator();

            if (punctuator != "\0")
            {
                _punctuators.Add(punctuator, type);
            }
        }

        // sort punctuators longest - smallest to make it possible to use symbols with more than one character
        OrderSymbols();
    }

    private void OrderSymbols()
    {
        _punctuators = new(_punctuators.OrderByDescending(_ => _.Key.Length));
    }

    public void AddSymbol(string symbol)
    {
        _punctuators.Add(symbol, PredefinedSymbols.Pool.Get(symbol));

        OrderSymbols();
    }

    public void AddMatcher(ILexerMatcher matcher)
    {
        _parts.Add(matcher);
    }

    public void MatchString(Symbol leftSymbol, Symbol rightSymbol)
    {
        AddMatcher(new StringMatcher(leftSymbol, rightSymbol));
    }

    public void MatchNumber(bool allowHex, bool allowBin, Symbol floatingPointSymbol = null,
        Symbol seperatorSymbol = null)
    {
        AddMatcher(new NumberMatcher(allowHex, allowBin, floatingPointSymbol ?? PredefinedSymbols.Dot,
            seperatorSymbol ?? PredefinedSymbols.Underscore));
    }

    public void Ignore(char c)
    {
        Ignore(new PunctuatorIgnoreMatcher(c.ToString()));
    }

    public void Ignore(string c)
    {
        Ignore(new PunctuatorIgnoreMatcher(c));
    }

    public void Ignore(ILexerIgnoreMatcher matcher)
    {
        _ignoreMatcher.Add(matcher);
    }

    public char Peek(int distance)
    {
        if (_index + distance >= Document.Source.Length)
        {
            return '\0';
        }

        return Document.Source[_index + distance];
    }


    public bool IsMatch(Symbol token)
    {
        if (string.IsNullOrEmpty(token.Name))
        {
            return false;
        }

        var result = Peek(0) == token.Name[0];

        for (var i = 1; i < token.Name.Length; i++)
        {
            if (result)
            {
                result = result && Peek(i) == token.Name[i];
            }
        }

        return result;
    }

    public Token Next()
    {
        while (_index < Document.Source.Length)
        {
            var c = Document.Source[_index];

            if (c == '\r')
            {
                _line++;
                _column = 1;
            }

            if (AdvanceIgnoreMatcher(c))
            {
                continue;
            }

            foreach (var part in _parts)
            {
                if (part.Match(this, c))
                {
                    return part.Build(this, ref _index, ref _column, ref _line).WithDocument(Document);
                }
            }

            foreach (var punctuator in _punctuators)
            {
                if (!IsMatch(punctuator.Key))
                {
                    continue;
                }

                return LexSymbol(punctuator.Key).WithDocument(Document);
            }

            if (char.IsLetter(c))
            {
                return LexName().WithDocument(Document);
            }

            Document.Messages.Add(Message.Error($"Invalid Character '{c}'"));

            return new Token("#invalid", c.ToString(), _line, _column).WithDocument(Document);
        }

        return new Token(PredefinedSymbols.EOF, "EndOfFile", _line, _column).WithDocument(Document);
    }

    private bool AdvanceIgnoreMatcher(char c)
    {
        foreach (var ignoreMatcher in _ignoreMatcher)
        {
            if (ignoreMatcher.Match(this, c))
            {
                ignoreMatcher.Advance(this);

                return true;
            }
        }

        return false;
    }

    private Token LexSymbol(string punctuatorKey)
    {
        var oldColumn = _column;

        _column += punctuatorKey.Length;
        _index += punctuatorKey.Length;

        return new(punctuatorKey, punctuatorKey, _line, oldColumn);
    }

    private Token LexName()
    {
        var oldColumn = _column;

        var start = _index;
        while (_index < Document.Source.Length)
        {
            if (!char.IsLetter(Document.Source[_index]))
            {
                break;
            }

            Advance();
        }

        var name = Document.Source.Substring(start, _index - start);

        if (_punctuators.ContainsKey(name))
        {
            return new(name, name, _line, oldColumn);
        }

        return new(PredefinedSymbols.Name, name, _line, oldColumn);
    }

    public void Advance(int distance = 1)
    {
        _index += distance;
        _column += distance;
    }

    public bool ContainsSymbol(string tokenName)
    {
        return _punctuators.ContainsKey(tokenName);
    }
}
