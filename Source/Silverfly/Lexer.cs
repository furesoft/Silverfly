using System;
using System.Text.RegularExpressions;
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

    /// <summary>
    /// Sets the source code from a <see cref="ReadOnlyMemory{T}"/> of characters and a specified filename.
    /// </summary>
    /// <param name="source">The source code to set.</param>
    /// <param name="filename">The name of the file containing the source code. Default is "tmp.synthetic".</param>
    public void SetSource(ReadOnlyMemory<char> source, string filename = "tmp.synthetic")
    {
        _index = -1;
        Document = new() { Filename = filename, Source = source };
    }
    
    /// <summary>
    /// Sets the source code from a string and a specified filename.
    /// </summary>
    /// <param name="source">The source code to set.</param>
    /// <param name="filename">The name of the file containing the source code. Default is "tmp.synthetic".</param>
    public void SetSource(string source, string filename = "tmp.synthetic")
    {
        SetSource(source.AsMemory(), filename);
    }

    /// <summary>
    /// Peeks at a character at a specified distance from the current index without advancing the index.
    /// </summary>
    /// <param name="distance">The distance from the current index to peek.</param>
    /// <returns>The character at the specified distance, or '\0' if the distance is out of range.</returns>
    public char Peek(int distance)
    {
        if (_index + distance >= Document.Source.Length)
        {
            return '\0';
        }

        return Document.Source.Slice(_index + distance, 1).Span[0];
    }

    /// <summary>
    /// Determines whether the current character is between two specified characters, inclusive.
    /// </summary>
    /// <param name="first">The lower bound character.</param>
    /// <param name="second">The upper bound character.</param>
    /// <returns><c>true</c> if the current character is between <paramref name="first"/> and <paramref name="second"/>; otherwise, <c>false</c>.</returns>
    public bool IsBetween(char first, char second) => Peek(0) >= first && Peek(0) <= second;

    /// <summary>
    /// Determines whether the current position in the document matches the specified token's name.
    /// </summary>
    /// <param name="token">The token to match against the document.</param>
    /// <param name="ignoreCase">Whether to ignore case when comparing the token's name. Default is <c>false</c>.</param>
    /// <returns><c>true</c> if the document matches the token's name at the current position; otherwise, <c>false</c>.</returns>
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
    
    /// <summary>
    /// Determines whether the current position in the document matches the specified regular expression.
    /// </summary>
    /// <param name="regex">The regular expression to match against the document.</param>
    /// <returns><c>true</c> if the document matches the regular expression at the current position; otherwise, <c>false</c>.</returns>
    public bool IsMatch(Regex regex)
    {
        var documentSlice = Document.Source.Slice(_index, Document.Source.Length - _index);
            
        return regex.IsMatch(documentSlice.Span);
    }

    /// <summary>
    /// Advances the lexer to the next token in the document.
    /// </summary>
    /// <returns>The next <see cref="Token"/> in the document.</returns>
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

            if (InvokeSymbols(out var next))
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

    private bool InvokeSymbols(out Token token)
    {
        foreach (var symbol in Config.Symbols)
        {
            if (!IsMatch(symbol.Key))
            {
                continue;
            }

            token = LexSymbol(symbol.Key, Document);
            return true;
        }

        token = default;
        return false;
    }

    private bool InvokeParts(char c, out Token token)
    {
        foreach (var part in Config.Matchers)
        {
            if (part.Match(this, c))
            {
                token = part.Build(this, ref _index, ref _column, ref _line);
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

        Advance(punctuatorKey.Length);

        return new(punctuatorKey, punctuatorKey.AsMemory(), _line, oldColumn, document);
    }

    private Token LexName(SourceDocument document)
    {
        var oldColumn = _column;

        var start = _index;

        Config.NameAdvancer.AdvanceName(this);

        var nameSlice = Document.Source[start.._index];
        var name = nameSlice.ToString();

        if (Config.Symbols.ContainsKey(name))
        {
            return new(name, nameSlice, _line, oldColumn, document);
        }

        return new(PredefinedSymbols.Name, nameSlice, _line, oldColumn, document);
    }

    /// <summary>
    /// Determines whether the lexer has not reached the end of the document.
    /// </summary>
    /// <returns><c>true</c> if the lexer is not at the end of the document; otherwise, <c>false</c>.</returns>
    public bool IsNotAtEnd() => _index < Document.Source.Length;
    
    /// <summary>
    /// Advances the current position in the document by a specified distance.
    /// </summary>
    /// <param name="distance">The number of characters to advance. Default is 1.</param>
    public void Advance(int distance = 1)
    {
        _index += distance;
        _column += distance;
    }

    /// <summary>
    /// Determines whether the specified token name is a punctuator.
    /// </summary>
    /// <param name="tokenName">The name of the token to check.</param>
    /// <returns><c>true</c> if the token name is a punctuator; otherwise, <c>false</c>.</returns>
    public bool IsPunctuator(string tokenName)
    {
        return Config.Symbols.ContainsKey(tokenName);
    }

    /// <summary>
    /// Determines whether the specified token name is a special token like start or end of document
    /// </summary>
    /// <param name="tokenName">The name of the token to check.</param>
    /// <returns><c>true</c> if the token name starts with a '#' and is not just "#"; otherwise, <c>false</c>.</returns>
    public bool IsSpecialToken(string tokenName)
    {
        return tokenName != "#" && tokenName.StartsWith('#');
    }
}
