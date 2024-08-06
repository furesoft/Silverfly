using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Silverfly.Lexing;
using Silverfly.Lexing.NameAdvancers;
using Silverfly.Text;

namespace Silverfly;

/// <summary>
/// Represents a lexer that tokenizes source code into meaningful tokens.
/// </summary>
public sealed partial class Lexer
{
    public LexerConfig Config;
    private int _index;
    private int _line = 1, _column = 1;

    public int CurrentIndex => _index;

    public SourceDocument Document { get; private set; }

    /// <summary>
    /// Creates a new <see cref="Lexer"/> to tokenize the given string.
    /// </summary>
    /// <param name="source">String to tokenize</param>
    public Lexer(LexerConfig config)
    {
        this.Config = config;
    }

    public void SetSource(ReadOnlyMemory<char> source, string filename = "tmp.synthetic")
    {
        _index = -1;
        Document = new() { Filename = filename, Source = source };
    }
    
    public void SetSource(string source, string filename = "tmp.synthetic")
    {
        SetSource(source.AsMemory(), filename);
    }

    public char Peek(int distance)
    {
        if (_index + distance >= Document.Source.Length)
        {
            return '\0';
        }

        return Document.Source.Slice(_index + distance, 1).Span[0];
    }

    public bool IsBetween(char first, char second) => Peek(0) >= first && Peek(0) <= second;

    public bool IsMatch(Symbol token, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(token.Name))
        {
            return false;
        }

        if (_index + token.Name.Length > Document.Source.Length)
        {
            return false;
        }

        var nameSpan = token.Name.AsMemory().Span;
        var documentSliceSpan = Document.Source.Slice(_index, token.Name.Length).Span;

        var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        return nameSpan.CompareTo(documentSliceSpan, comparisonType) == 0;
    }
    
    public bool IsMatch(Regex regex)
    {
        var documentSlice = Document.Source.Slice(_index, Document.Source.Length - _index);
            
        return regex.IsMatch(documentSlice.Span);
    }

    public Token Next()
    {
        if (_index == -1)
        {
            _index++;

            return new Token(PredefinedSymbols.SOF, _line, _column, Document);
        }

        while (IsNotAtEnd())
        {
            var c = Peek(0);

            RecognizeLine(c);

            if (AdvanceIgnoreMatcher(c))
            {
                continue;
            }

            if (InvokeParts(c, out var token))
            {
                return token;
            }

            if (Config.NameAdvancer.IsNameStart(c))
            {
                return LexName(Document);
            }

            if (InvokePunctuators(out var next))
            {
                return next;
            }

            Document.Messages.Add(Message.Error($"Unknown Character '{c}'", SourceRange.From(Document, _line, _column, _line, _column)));

            return Token.Invalid(c, _line, _column, Document);
        }

        return new Token(PredefinedSymbols.EOF, _line, _column, Document);
    }

    private void RecognizeLine(char c)
    {
        if (c != '\r')
        {
            return;
        }

        _line++;
        _column = 1;
    }

    private bool InvokePunctuators(out Token token)
    {
        foreach (var punctuator in Config.Punctuators)
        {
            if (!IsMatch(punctuator.Key))
            {
                continue;
            }

            token = LexSymbol(punctuator.Key, Document);
            return true;
        }

        token = default;
        return false;
    }

    private bool InvokeParts(char c, out Token token)
    {
        foreach (var part in Config.Parts)
        {
            if (part.Match(this, c))
            {
                token = part.Build(this, ref _index, ref _column, ref _line, Document);
                return true;
            }
        }

        token = default;
        return false;
    }

    private bool AdvanceIgnoreMatcher(char c)
    {
        foreach (var ignoreMatcher in Config.IgnoreMatchers)
        {
            if (ignoreMatcher.Match(this, c))
            {
                ignoreMatcher.Advance(this);

                return true;
            }
        }

        return false;
    }

    private Token LexSymbol(string punctuatorKey, SourceDocument document)
    {
        var oldColumn = _column;

        _column += punctuatorKey.Length;
        _index += punctuatorKey.Length;

        return new(punctuatorKey, _line, oldColumn, document);
    }

    private Token LexName(SourceDocument document)
    {
        var oldColumn = _column;

        var start = _index;

        Config.NameAdvancer.AdvanceName(this);

        var nameSlice = Document.Source[start.._index];
        var name = nameSlice.ToString();

        if (Config.Punctuators.ContainsKey(name))
        {
            return new(name, nameSlice, _line, oldColumn, document);
        }

        return new(PredefinedSymbols.Name, nameSlice, _line, oldColumn, document);
    }

    public bool IsNotAtEnd() => _index < Document.Source.Length;

    public void Advance(int distance = 1)
    {
        _index += distance;
        _column += distance;
    }

    public bool IsPunctuator(string tokenName)
    {
        return Config.Punctuators.ContainsKey(tokenName);
    }
}
