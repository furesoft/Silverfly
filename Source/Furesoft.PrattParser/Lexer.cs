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
    private readonly List<char> _ignoredChars = new();
    private readonly string _source;
    private int _index;
    private int _line = 1, _column = 1;

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
        
        _punctuators = new(_punctuators.OrderByDescending(_ => _.Key.Length));
    }

    public void AddSymbol(string symbol)
    {
        _punctuators.Add(symbol, PredefinedSymbols.Pool.Get(symbol));
    }

    public void AddKeyword(string keyword)
    {
        _keywords.Add(PredefinedSymbols.Pool.Get(keyword));
    }

    public void Ignore(char c)
    {
        _ignoredChars.Add(c);
    }

    public void AddKeywords(params string[] keywords)
    {
        _keywords.AddRange(keywords.Select(_ => PredefinedSymbols.Pool.Get(_)));
    }

    private char Peek(int distance)
    {
        if (_index + distance >= _source.Length)
        {
            return '\0';
        }

        return _source[_index + distance];
    }

    private bool IsMatch(string token)
    {
        bool result = Peek(1) == token[0]; //ToDo: turn peek(1) to peek(0). need to be figured out where index++ is missing!!

        for (int i = 1; i < token.Length; i++)
        {
            if (result)
            {
                result = result && Peek(i+1) == token[i]; //ToDo: turn peek(i+11) to peek(i). need to be figured out where index++ is missing!!
            }
        }

        return result;
    }

    public Token Next()
    {
        while (_index < _source.Length)
        {
            var c = _source[_index++];

            if (c == '\r')
            {
                _line++;
                _column = 1;
            }

            if (_ignoredChars.Contains(c))
            {
                _column++;
                _index++;
                
                continue;
            }

            foreach (var punctuator in _punctuators)
            {
                if (IsMatch(punctuator.Key))
                {
                    return LexSymbol(punctuator.Key);
                }
            }
            
            if (_punctuators.TryGetValue(c.ToString(), out var symbol))
            {
                _column++;

                return new(symbol, c.ToString(), _line, _column);
            }

            if (char.IsDigit(c))
            {
                return LexNumber();
            }

            if (char.IsLetter(c))
            {
                return LexName();
            }

            return new(PredefinedSymbols.Pool.Get("#invalid"), c.ToString(), _line, _column);
        }

        return new(PredefinedSymbols.EOF, string.Empty, _line, _column);
    }

    private Token LexSymbol(string punctuatorKey)
    {
        _column += punctuatorKey.Length;
        _index += punctuatorKey.Length;

        return new(PredefinedSymbols.Pool.Get(punctuatorKey), punctuatorKey, _line, _column);
    }

    private Token LexName()
    {
        var start = _index - 1;
        while (_index < _source.Length)
        {
            if (!char.IsLetter(_source[_index]))
            {
                break;
            }

            _index++;
            _column++;
        }

        var name = _source.Substring(start, _index - start);

        if (_keywords.Any(_ => _.Name == name))
        {
            return new(PredefinedSymbols.Pool.Get(name), name, _line, _column);
        }

        return new(PredefinedSymbols.Name, name, _line, _column);
    }

    private Token LexNumber()
    {
        var startIndex = _index - 1;

        while (_index < _source.Length)
        {
            if (!char.IsDigit(_source[_index]))
            {
                break;
            }

            _column++;
            _index++;
        }

        return new(PredefinedSymbols.Integer, _source.Substring(startIndex, _index - startIndex), _line, _column);
    }
}
