#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Silverfly.Helpers;
using Silverfly.Nodes;
using Silverfly.Text;
using Silverfly.Text.Formatting;

namespace Silverfly;

//Todo: Add Synchronisation Mechanism For Better Error Reporting
public abstract partial class Parser
{
    public readonly Lexer Lexer;
    public readonly ParserDefinition ParserDefinition = new();
    public ParserOptions Options = new(true, true);
    private readonly List<Token> _read = [];
    public readonly MessageFormatter Formatter;

    /// <summary>
    /// Gets the <see cref="SourceDocument"/> associated with the lexer.
    /// </summary>
    /// <value>The <see cref="SourceDocument"/> that the lexer is currently working with.</value>
    /// <remarks>
    /// This property provides access to the <see cref="SourceDocument"/> instance associated with the lexer.
    /// It allows retrieval of the document's contents and messages.
    /// </remarks>
    public SourceDocument Document => Lexer.Document;

    /// <summary>
    /// Parses a statement and optionally wraps expressions in an AST node.
    /// </summary>
    /// <param name="wrapExpressions">Indicates whether to wrap expressions in an AST node. Default is false.</param>
    /// <returns>The parsed abstract syntax tree (AST) node representing the statement.</returns>
    public AstNode ParseStatement(bool wrapExpressions = false)
    {
        var token = LookAhead();

        if (ParserDefinition._statementParselets.TryGetValue(token.Type, out var parselet))
        {
            Consume();

            return parselet.Parse(this, token);
        }

        var expression = ParseExpression();

        if (wrapExpressions)
        {
            var exprStmt = new ExpressionStatement(expression);
            expression.WithParent(exprStmt);

            return exprStmt;
        }

        return expression;
    }

    /// <summary>
    /// Parses a source code string and returns the corresponding translation unit.
    /// </summary>
    /// <param name="source">The source code to parse as a string.</param>
    /// <param name="filename">The name of the file from which the source code was read. Default is "synthetic.dsl".</param>
    /// <returns>The parsed translation unit representing the source code.</returns>
    public TranslationUnit Parse(string source, string filename = "synthetic.dsl")
    {
        return Parse(source.AsMemory(), filename);
    }

    public Parser()
    {
        var lexerConfig = new LexerConfig();
        InitLexer(lexerConfig);

        Lexer = new Lexer(lexerConfig);
        
        InitParser(ParserDefinition);

        AddLexerSymbols(Lexer, ParserDefinition._prefixParselets);
        AddLexerSymbols(Lexer, ParserDefinition._infixParselets);
        AddLexerSymbols(Lexer, ParserDefinition._statementParselets);

        Lexer.Config.OrderSymbols();

        Formatter = new(this);
    }

    /// <summary>
    /// Parses a source code and returns the corresponding translation unit.
    /// </summary>
    /// <param name="source">The source code to parse as a <see cref="ReadOnlyMemory{char}"/>.</param>
    /// <param name="filename">The name of the file from which the source code was read. Default is "synthetic.dsl".</param>
    /// <returns>The parsed translation unit representing the source code.</returns>
    public TranslationUnit Parse(ReadOnlyMemory<char> source, string filename = "synthetic.dsl")
    {
        Lexer.SetSource(source, filename);

        var node = Options.UseStatementsAtToplevel
            ? ParseStatement()
            : ParseExpression();

        if (Options.EnforceEndOfFile)
        {
            Match(PredefinedSymbols.EOF);
        }

        return new TranslationUnit(node, Lexer.Document);
    }

    private void AddLexerSymbols<TParselet>(Lexer lexer, Dictionary<Symbol, TParselet> dict)
    {
        foreach (var parselet in dict)
        {
            if (!lexer.IsPunctuator(parselet.Key.Name) && !lexer.IsSpecialToken(parselet.Key.Name))
            {
                lexer.Config.AddSymbol(parselet.Key.Name);
            }

            if (parselet is ISymbolDiscovery sd)
            {
                _ = sd.GetSymbols().All(s =>
                {
                    lexer.Config.AddSymbol(s.Name);
                    return true;
                });
            }
        }
    }

    /// <summary>
    /// Parses an expression based on the given precedence level.
    /// </summary>
    /// <param name="precedence">The precedence level used for parsing the expression.</param>
    /// <returns>The parsed abstract syntax tree (AST) node representing the expression.</returns>
    public AstNode Parse(int precedence)
    {
        var token = Consume();

        if (token.Type == "#invalid" || token.Type == PredefinedSymbols.EOF)
        {
            return new InvalidNode(token).WithRange(token);
        }

        if (!ParserDefinition._prefixParselets.TryGetValue(token.Type, out var prefix))
        {
            token.Document.AddMessage(MessageSeverity.Error, $"Failed to parse token '{token.Text}'", token.GetRange());

            return new InvalidNode(token).WithRange(token);
        }

        var left = prefix.Parse(this, token);

        while (precedence < GetBindingPower())
        {
            token = Consume();

            if (!ParserDefinition._infixParselets.TryGetValue(token.Type, out var infix))
            {
                token.Document.Messages.Add(
                    Message.Error($"Failed to parse token '{token.Text}'", token.GetRange()));

                return new InvalidNode(token).WithRange(token);
            }

            left = infix!.Parse(this, left, token);
        }

        return left;
    }

    /// <summary>
    /// Parses an expression
    /// </summary>
    /// <returns>The parsed abstract syntax tree (AST) node.</returns>
    public AstNode ParseExpression()
    {
        if (IsMatch(PredefinedSymbols.SOF))
        {
            Consume(PredefinedSymbols.SOF);
        }

        return Parse(0);
    }

    /// <summary>
    /// Parses a typename
    /// </summary>
    /// <returns></returns>
    public TypeName? ParseTypeName()
    {
        return ParserDefinition._typeNameParser.TryParse(this, out var typename) ? typename : null;
    }

    /// <summary>
    /// Initializes the lexer with the specified configuration.
    /// </summary>
    /// <param name="lexer">The configuration settings for initializing the lexer.</param>
    /// <remarks>
    /// This abstract method must be implemented in derived classes to provide custom initialization logic for the lexer.
    /// The <paramref name="lexer"/> parameter contains the configuration settings that define how the lexer should be set up.
    /// </remarks>
    protected abstract void InitLexer(LexerConfig lexer);
    
    /// <summary>
    /// Initializes the parser with the specified parser definition.
    /// </summary>
    /// <param name="def">The definition settings for initializing the parser.</param>
    /// <remarks>
    /// This abstract method must be implemented in derived classes to provide custom initialization logic for the parser.
    /// The <paramref name="def"/> parameter contains the settings and rules that define how the parser should be configured.
    /// </remarks>
    protected abstract void InitParser(ParserDefinition def);

    /// <summary>
    /// Checks if the current symbol matches the expected symbol and consumes it if it does.
    /// </summary>
    /// <param name="expected">The expected symbol to match.</param>
    /// <returns>True if the expected symbol matches and is consumed; otherwise, false.</returns>
    public bool Match(Symbol expected)
    {
        if (!IsMatch(expected))
        {
            return false;
        }

        Consume(expected);

        return true;
    }

    /// <summary>
    /// Checks if any of the expected symbols match the current symbol.
    /// </summary>
    /// <param name="expected">An array of expected symbols to match.</param>
    /// <returns>True if any of the expected symbols match; otherwise, false.</returns>
    public bool Match(params Symbol[] expected)
    {
        foreach (var symbol in expected)
        {
            if (Match(symbol))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the symbol at a specified distance matches the expected symbol.
    /// </summary>
    /// <param name="expected">The expected symbol to match.</param>
    /// <param name="distance">The distance ahead to look for the symbol (default is 0).</param>
    /// <returns>True if the symbol at the specified distance matches the expected symbol; otherwise, false.</returns>
    public bool IsMatch(Symbol expected, uint distance = 0)
    {
        EnsureSymbolIsRegistered(expected);

        var token = LookAhead(distance);

        return CompareToken(token, expected);
    }

    private void EnsureSymbolIsRegistered(Symbol expected)
    {
        if (!Lexer.IsPunctuator(expected.Name))
        {
            Lexer.Config.AddSymbol(expected.Name);
        }
    }

    /// <summary>
    /// Checks if a sequence of symbols matches the expected symbols.
    /// </summary>
    /// <param name="expected">An array of expected symbols to match.</param>
    /// <returns>True if the sequence of symbols matches; otherwise, false.</returns>
    public bool IsMatch(params Symbol[] expected)
    {
        var result = true;
        for (uint i = 0; i < expected.Length; i++)
        {
            result &= IsMatch(expected[i], i);
        }

        return result;
    }

    private bool CompareToken(Token token, Symbol expected)
    {
        return token.Type.Name.AsSpan().CompareTo(expected.Name.AsSpan(), Lexer.Config.Casing) == 0;
    }

    /// <summary>
    /// If the <paramref name="expected"/> symbol is matched, consume the token.
    /// </summary>
    /// <param name="expected">The expected symbol to match.</param>
    /// <returns>The consumed token if the expected symbol is matched; otherwise, an invalid token.</returns>
    public Token Consume(Symbol expected)
    {
        var token = LookAhead();

        EnsureSymbolIsRegistered(expected);
        
        if (!CompareToken(token, expected))
        {
            token.Document.Messages.Add(
                Message.Error($"Expected token {expected} found {token.Type}({token})", token.GetRange()));

            return Token.Invalid('\0', token.Line, token.Column, Document);
        }

        return Consume();
    }

    /// <summary>
    /// Returns the next <see cref="Token"/> and removes it from the read list.
    /// </summary>
    /// <returns>The next <see cref="Token"/>.</returns>
    public Token Consume()
    {
        var token = LookAhead();

        _read.RemoveAt(0);

        return token;
    }

    /// <summary>
    /// Consumes as many tokens as given in <paramref name="count" />.
    /// </summary>
    /// <param name="count">The number of tokens to consume.</param>
    /// <returns>An array of consumed tokens.</returns>
    public Token[] ConsumeMany(uint count)
    {
        var result = new List<Token>();
        for (var i = 0; i < count; i++)
        {
            result.Add(Consume());
        }

        return [.. result];
    }

    /// <summary>
    /// Get the next token(s) and add it to a cache to reuse it later.
    /// </summary>
    /// <param name="distance">The number of tokens to look ahead.</param>
    /// <returns>The token at the specified distance.</returns>
    public Token LookAhead(uint distance = 0)
    {
        // Read in as many as needed.
        while (distance >= _read.Count)
        {
            _read.Add(Lexer.Next());
        }

        // Get the queued token.
        return _read[(int)distance];
    }
    
    /// <summary>
    /// Prints all messages stored in the message list using the <see cref="MessageFormatter"/>.
    /// </summary>
    public void PrintMessages()
    {
        foreach (var message in Document.Messages)
        {
            Formatter.PrintError(CompilerError.FromMessage(message));
        }

        Console.ResetColor();
    }

    private int GetBindingPower()
    {
        if (ParserDefinition._infixParselets.TryGetValue(LookAhead().Type, out var parselet))
        {
            return parselet.GetBindingPower();
        }

        return 0;
    }
}
