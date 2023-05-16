using System.Collections.Generic;
using System.Linq;

namespace Furesoft.PrattParser;

/// <summary>
/// A very primitive lexer. Takes a string and splits it into a series of Tokens.
/// Operators and punctuation are mapped to unique keywords. Names, which can be
/// any series of letters, are turned into NAME tokens. All other characters are
/// ignored (except to separate names). Numbers and strings are not supported. This
/// is really just the bare minimum to give the parser something to work with.
/// </summary>
public class Lexer : ILexer
{
    private readonly Dictionary<string, Symbol> _punctuators = new();
    private readonly List<Symbol> _keywords = new();
    private readonly string _source;
    private int _index;

    /// <summary>
    /// Creates a new <see cref="Lexer"/> to tokenize the given string.
    /// </summary>
    /// <param name="text">String to tokenize</param>
    public Lexer(string text)
    {
        _index = 0;
        _source = text;

        // Register all of the Symbols that are explicit punctuators.
        foreach (var type in PredefinedSymbols.Pool)
        {
            var punctuator = type.Punctuator();

            if (punctuator != "\0")
            {
                _punctuators.Add(punctuator, type);
            }
        }
    }

    public void AddSymbol(string symbol)
    {
        _punctuators.Add(symbol, PredefinedSymbols.Pool.Get(symbol));
    }

    public void AddKeyword(string keyword)
    {
        _keywords.Add(PredefinedSymbols.Pool.Get(keyword));
    }

    public void AddKeywords(params string[] keywords)
    {
        _keywords.AddRange(keywords.Select(_ => PredefinedSymbols.Pool.Get(_)));
    }

    public Token Next()
    {
        while (_index < _source.Length)
        {
            var c = _source[_index++];

            if (_punctuators.TryGetValue(c.ToString(), out var symbol))
            {
                return new(symbol, char.ToString(c));
            }

            if (char.IsDigit(c))
            {
                return new(PredefinedSymbols.Integer, LexNumber());
            }

            if (char.IsLetter(c))
            {
                // Handle names.
                var start = _index - 1;
                while (_index < _source.Length)
                {
                    if (!char.IsLetter(_source[_index]))
                    {
                        break;
                    }

                    _index++;
                }

                var name = _source.Substring(start, _index - start);

                if (_keywords.Any(_ => _.Name == name))
                {
                    return new(PredefinedSymbols.Pool.Get(name), name);
                }

                return new(PredefinedSymbols.Name, name);
            }
            else
            {
                // Ignore all other characters (whitespace, etc.)
            }
        }

        return new(PredefinedSymbols.EOF, string.Empty);
    }

    private string LexNumber()
    {
        int startIndex = _index;

        while (char.IsDigit(_source[_index]))
        {
            _index++;
        }

        return _source.Substring(startIndex, _index);
    }
}
