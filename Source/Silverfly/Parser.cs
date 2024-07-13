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
    private readonly Dictionary<Symbol, IStatementParselet> _statementParselets = [];
    private readonly List<Token> _read = [];
    private Lexer _lexer;

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

    public void Block(Symbol start, Symbol terminator, Symbol seperator = null, bool wrapExpressions = false, Symbol tag = null)
    {
        Register(start, new BlockParselet(terminator, seperator, wrapExpressions, tag));
    }

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

    public static TranslationUnit Parse<TParser>(string source, string filename = "syntethic.dsl",
        bool useStatementsAtToplevel = false, bool enforceEndOfFile = true)
        where TParser : Parser, new()
    {
        var lexer = new Lexer(source, filename);

        var parser = new TParser { _lexer = lexer };

        parser.InitParselets();

        AddLexerSymbols(lexer, parser._prefixParselets);
        AddLexerSymbols(lexer, parser._infixParselets);
        AddLexerSymbols(lexer, parser._statementParselets);

        parser.InitLexer(lexer);

        lexer.OrderSymbols();

        var node = useStatementsAtToplevel
                            ? parser.ParseStatement()
                            : parser.ParseExpression();

        if (enforceEndOfFile)
        {
            parser.Match(PredefinedSymbols.EOF);
        }

        return new(node, lexer.Document);
    }

    private static void AddLexerSymbols<TParselet>(Lexer lexer, Dictionary<Symbol, TParselet> dict)
    {
        foreach (var prefix in dict)
        {
            if (!lexer.IsPunctuator(prefix.Key.Name) && !prefix.Key.Name.StartsWith('#'))
            {
                lexer.AddSymbol(prefix.Key.Name);
            }
        }
    }

    public AstNode Parse(int precedence)
    {
        var token = Consume();

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

            left = infix.Parse(this, left, token);
        }

        return left;
    }

    public AstNode ParseExpression()
    {
        if (IsMatch(PredefinedSymbols.SOF))
        {
            Consume(PredefinedSymbols.SOF);
        }

        return Parse(0);
    }

    protected abstract void InitLexer(Lexer lexer);
    protected abstract void InitParselets();

    public bool Match(Symbol expected)
    {
        if (!IsMatch(expected))
        {
            return false;
        }

        Consume();

        return true;
    }

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
            _lexer.AddSymbol(expected.Name);
        }
    }

    /// <summary>Checks if a sequence of symbols match
    public bool IsMatch(params Symbol[] expected)
    {
        var result = true;
        for (uint i = 0; i < expected.Length; i++)
        {
            result &= IsMatch(expected[i], i);
        }

        return result;
    }

    /// <summary>If the <paramref name="expected"/> is matched consume it
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

    /// <summary>Returns the next <see cref="Token"/>
    public Token Consume()
    {
        // Make sure we've read the token.
        var token = LookAhead(0);
        _read.Remove(token);

        return token;
    }

    /// <summary>Consumes as many tokens as given in <paramref name="count"/></summary>
    public Token[] ConsumeMany(uint count)
    {
        var result = new List<Token>();
        for (var i = 0; i < count; i++)
        {
            result.Add(Consume());
        }

        return [.. result];
    }

    /// <summary>Get the next token(s) and add it to a cache to reuse it later</summary>
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
        Register(firstSymbol, new TernaryOperatorParselet(secondSymbol, PrecedenceLevels.GetPrecedence(bindingPowerName)));
    }
}
