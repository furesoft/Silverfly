using System;
using System.Collections.Generic;
using Silverfly.Nodes;
using Silverfly.Parselets;
using Silverfly.Parselets.Operators;
using Silverfly.Text;

namespace Silverfly;

//Todo: Add Synchronisation Mechanism For Better Error Reporting
public abstract partial class Parser
{
    private readonly Dictionary<Symbol, IInfixParselet> _infixParselets = [];
    private readonly Dictionary<Symbol, IPrefixParselet> _prefixParselets = [];
    private readonly List<Token> _read = [];
    private readonly Dictionary<Symbol, IStatementParselet> _statementParselets = [];
    private Lexer _lexer;
    private LexerConfig _lexerConfig = new();
    protected ParserOptions Options = new(true, true);

    public PrecedenceLevels PrecedenceLevels = new DefaultPrecedenceLevels();

    public SourceDocument Document => _lexer.Document;

    public void Register(Symbol token, IPrefixParselet parselet)
    {
        _prefixParselets[token] = parselet;
    }

    public void Register(Symbol token, IInfixParselet parselet)
    {
        _infixParselets[token] = parselet;
    }

    public void Register(Symbol token, IStatementParselet parselet)
    {
        _statementParselets[token] = parselet;
    }

    public void Register(IInfixParselet parselet, params Symbol[] tokens)
    {
        foreach (var token in tokens)
        {
            Register(token, parselet);
        }
    }

    public void Register(IPrefixParselet parselet, params Symbol[] tokens)
    {
        foreach (var token in tokens)
        {
            Register(token, parselet);
        }
    }

    public void Group(Symbol leftToken, Symbol rightToken, Symbol tag = null)
    {
        Register(leftToken, new GroupParselet(leftToken, rightToken, tag));
    }

    public void Block(Symbol start, Symbol terminator, Symbol seperator = null, bool wrapExpressions = false,
        Symbol tag = null)
    {
        Register(start, new BlockParselet(terminator, seperator, wrapExpressions, tag));
    }

    /// <summary>
    /// Parses a statement and optionally wraps expressions in an AST node.
    /// </summary>
    /// <param name="wrapExpressions">Indicates whether to wrap expressions in an AST node. Default is false.</param>
    /// <returns>The parsed abstract syntax tree (AST) node representing the statement.</returns>
    public AstNode ParseStatement(bool wrapExpressions = false)
    {
        var token = LookAhead(0);

        if (_statementParselets.TryGetValue(token.Type, out var parselet))
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
    public TranslationUnit Parse(string source, string filename = "syntethic.dsl")
    {
        return Parse(source.AsMemory(), filename);
    }

    public Parser()
    {
        InitLexer(_lexerConfig);
    }

    /// <summary>
    /// Parses a source code and returns the corresponding translation unit.
    /// </summary>
    /// <param name="source">The source code to parse as a <see cref="ReadOnlyMemory{char}"/>.</param>
    /// <param name="filename">The name of the file from which the source code was read. Default is "synthetic.dsl".</param>
    /// <returns>The parsed translation unit representing the source code.</returns>
    public TranslationUnit Parse(ReadOnlyMemory<char> source, string filename = "syntethic.dsl")
    {
        _lexer = new Lexer(source, _lexerConfig, filename);

        InitParselets();

        AddLexerSymbols(_lexer, _prefixParselets);
        AddLexerSymbols(_lexer, _infixParselets);
        AddLexerSymbols(_lexer, _statementParselets);

        _lexer.Config.OrderSymbols();

        var node = Options.UseStatementsAtToplevel
            ? ParseStatement()
            : ParseExpression();

        if (Options.EnforceEndOfFile)
        {
            Match(PredefinedSymbols.EOF);
        }

        return new TranslationUnit(node, _lexer.Document);
    }

    private void AddLexerSymbols<TParselet>(Lexer lexer, Dictionary<Symbol, TParselet> dict)
    {
        foreach (var prefix in dict)
        {
            if (!lexer.IsPunctuator(prefix.Key.Name) && !prefix.Key.Name.StartsWith('#'))
            {
                _lexerConfig.AddSymbol(prefix.Key.Name);
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

        if (!_prefixParselets.TryGetValue(token.Type, out var prefix))
        {
            token.Document.Messages.Add(Message.Error("Could not parse prefix \"" + token.Text + "\".",
                token.GetRange()));

            return new InvalidNode(token).WithRange(token);
        }

        var left = prefix.Parse(this, token);

        while (precedence < GetBindingPower())
        {
            token = Consume();

            if (!_infixParselets.TryGetValue(token.Type, out var infix))
            {
                token.Document.Messages.Add(
                    Message.Error("Could not parse \"" + token.Text + "\".", token.GetRange()));
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

    protected abstract void InitLexer(LexerConfig lexer);
    protected abstract void InitParselets();

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
    public bool Match(Symbol[] expected)
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

        return token.Type.Equals(expected);
    }

    private void EnsureSymbolIsRegistered(Symbol expected)
    {
        if (!_lexer.IsPunctuator(expected.Name))
        {
            _lexerConfig.AddSymbol(expected.Name);
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

    /// <summary>
    /// If the <paramref name="expected"/> symbol is matched, consume the token.
    /// </summary>
    /// <param name="expected">The expected symbol to match.</param>
    /// <returns>The consumed token if the expected symbol is matched; otherwise, an invalid token.</returns>
    public Token Consume(Symbol expected)
    {
        var token = LookAhead(0);

        EnsureSymbolIsRegistered(expected);

        if (!token.Type.Equals(expected))
        {
            token.Document.Messages.Add(
                Message.Error($"Expected token {expected} and found {token.Type}({token})", token.GetRange()));

            return Token.Invalid('\0', token.Line, token.Column);
        }

        return Consume();
    }

    /// <summary>
    /// Returns the next <see cref="Token"/> and removes it from the read list.
    /// </summary>
    /// <returns>The next <see cref="Token"/>.</returns>
    public Token Consume()
    {
        // Make sure we've read the token.
        var token = LookAhead(0);
        _read.Remove(token);

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
    public Token LookAhead(uint distance)
    {
        // Read in as many as needed.
        while (distance >= _read.Count)
        {
            _read.Add(_lexer.Next());
        }

        // Get the queued token.
        return _read[(int)distance];
    }

    private int GetBindingPower()
    {
        if (_infixParselets.TryGetValue(LookAhead(0).Type, out var parselet))
        {
            return parselet.GetBindingPower();
        }

        return 0;
    }

    /// <summary>
    ///     Registers a postfix unary operator parselet for the given token and binding power.
    /// </summary>
    protected void Postfix(Symbol token, string bindingPowerName = "Postfix")
    {
        Register(token, new PostfixOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName)));
    }

    /// <summary>
    ///     Registers a prefix unary operator parselet for the given token and binding power.
    /// </summary>
    protected void Prefix(Symbol token, string bindingPowerName = "Prefix")
    {
        Register(token, new PrefixOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName)));
    }

    /// <summary>
    ///     Registers a left-associative binary operator parselet for the given token and binding power.
    /// </summary>
    protected void InfixLeft(Symbol token, string bindingPowerName)
    {
        Register(token, new BinaryOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName), false));
    }

    /// <summary>
    ///     Registers a right-associative binary operator parselet for the given token and binding power.
    /// </summary>
    protected void InfixRight(Symbol token, string bindingPowerName)
    {
        Register(token, new BinaryOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName), true));
    }

    /// <summary>
    ///     Register a ternary operator like the :? operator
    /// </summary>
    /// <param name="firstSymbol"></param>
    /// <param name="secondSymbol"></param>
    /// <param name="bindingPower"></param>
    protected void Ternary(Symbol firstSymbol, Symbol secondSymbol, string bindingPowerName)
    {
        Register(firstSymbol,
            new TernaryOperatorParselet(secondSymbol, PrecedenceLevels.GetPrecedence(bindingPowerName)));
    }
}
