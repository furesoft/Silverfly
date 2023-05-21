using System.Collections.Generic;
using System.Linq;

namespace Furesoft.PrattParser;

public class Lexer : ILexer
{
    private readonly Dictionary<string, Symbol> _punctuators = new();
    private readonly List<char> _ignoredChars = new();
    private int _index;
    private int _line = 1, _column = 1;
    private Symbol leftStr, rightStr;

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
        _punctuators = new(_punctuators.OrderByDescending(_ => _.Key.Length));
    }

    public void AddSymbol(string symbol)
    {
        _punctuators.Add(symbol, PredefinedSymbols.Pool.Get(symbol));
    }

    public void UseString(Symbol leftSymbol, Symbol rightSymbol)
    {
        leftStr = leftSymbol;
        rightStr = rightSymbol;
    }

    public void Ignore(char c)
    {
        _ignoredChars.Add(c);
    }

    private char Peek(int distance)
    {
        if (_index + distance >= Document.Source.Length)
        {
            return '\0';
        }

        return Document.Source[_index + distance];
    }

    private bool IsMatch(string token)
    {
        var result = Peek(0) == token[0];

        for (var i = 1; i < token.Length; i++)
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
        while (_index < Document.Source.Length)
        {
            var c = Document.Source[_index];

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

            if (leftStr != null && rightStr != null)
            {
                if (IsMatch(leftStr.Name))
                {
                    return LexString();
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

            if (char.IsDigit(c))
            {
                return LexNumber().WithDocument(Document);
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

    private Token LexString()
    {
        var oldColumn = _column;
        var oldIndex = _index;
        
        _index++;
        _column++;

        while (!IsMatch(rightStr.Name))
        {
            _index++;
            _column++;
        }

        var text = Document.Source.Substring(oldIndex +1, _index - oldIndex - 1);
        
        _index++;
        _column++;

        
        return new(PredefinedSymbols.String, text, _line, oldColumn);
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

            _index++;
            _column++;
        }

        var name = Document.Source.Substring(start, _index - start);

        if (_punctuators.ContainsKey(name))
        {
            return new(name, name, _line, oldColumn);
        }

        return new(PredefinedSymbols.Name, name, _line, oldColumn);
    }

    private Token LexNumber()
    {
        var oldColumn = _column;
        var startIndex = _index;

        while (_index < Document.Source.Length)
        {
            if (!char.IsDigit(Document.Source[_index]))
            {
                break;
            }

            _column++;
            _index++;
        }

        return new(PredefinedSymbols.Integer, Document.Source.Substring(startIndex, _index - startIndex), _line,
            oldColumn);
    }
}
