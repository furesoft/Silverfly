using System.Collections.Generic;
using System.Linq;

namespace Furesoft.PrattParser;

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

    public bool IsKeyword(Symbol symbol)
    {
        return _keywords.Contains(symbol);
    }

    private bool IsMatch(string token)
    {
        bool result = Peek(0) == token[0];

        for (int i = 1; i < token.Length; i++)
        {
            if (result)
            {
                result = result && Peek(i) == token[i];
            }
        }

        return result;
    }

    public Token Next()
    {
        while (_index < _source.Length)
        {
            var c = _source[_index];

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
                if (!IsMatch(punctuator.Key))
                {
                    continue;
                }
                
                return LexSymbol(punctuator.Key);
            }

            if (char.IsDigit(c))
            {
                return LexNumber();
            }

            if (char.IsLetter(c))
            {
                return LexName();
            }

            return new("#invalid", c.ToString(), _line, _column);
        }

        return new(PredefinedSymbols.EOF, "EndOfFile", _line, _column);
    }

    private Token LexSymbol(string punctuatorKey)
    {
        _column += punctuatorKey.Length;
        _index += punctuatorKey.Length;
        
        return new(punctuatorKey, punctuatorKey, _line, _column);
    }

    private Token LexName()
    {
        var start = _index;
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
            return new(name, name, _line, _column);
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
