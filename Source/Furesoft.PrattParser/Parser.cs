using System.Collections.Generic;
using System.Xml;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Parselets.Builder;
using Furesoft.PrattParser.Parselets.Builder.Elements;
using Furesoft.PrattParser.Parselets.Operators;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser;

public abstract class Parser
{
    private readonly Dictionary<Symbol, IInfixParselet> _infixParselets = [];
    private readonly Dictionary<Symbol, IPrefixParselet> _prefixParselets = [];
    private readonly Dictionary<Symbol, IStatementParselet> _statementParselets = [];
    private readonly List<Token> _read = [];
    private Lexer _lexer;

    public SourceDocument Document => _lexer.Document;

    public void Register(Symbol token, IPrefixParselet parselet)
    {
        _prefixParselets.Add(token, parselet);
    }

    public void Register(Symbol token, IInfixParselet parselet)
    {
        _infixParselets.Add(token, parselet);
    }

    public void Register(Symbol token, IStatementParselet parselet) {
        _statementParselets.Add(token, parselet);
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

    public void Group(Symbol leftToken, Symbol rightToken)
    {
        Register(leftToken, new GroupParselet(rightToken));
    }

    public void Block(Symbol start, Symbol terminator, Symbol seperator = null, bool wrapExpressions = false)
    {
        Register(start, new BlockParselet(terminator, seperator, wrapExpressions));
    }

    public void ExprBuilder<TNode>(SyntaxElement definition)
        where TNode : AstNode
    {
        Builder<TNode>(definition, NodeType.Expression);
    }

    public void StmtBuilder<TNode>(SyntaxElement definition)
        where TNode : AstNode
    {
        Builder<TNode>(definition, NodeType.Statement);
    }


    private void Builder<TNode>(SyntaxElement definition, NodeType type)
        where TNode : AstNode
    {
        var parselet = new BuilderParselet<TNode>(BindingPower.Product, definition);
        var keyword = GetKeyword(definition);

        if (keyword != null)
        {
            switch (type)
            {
                default:
                case NodeType.Expression:
                    Register(keyword, (IInfixParselet)parselet);
                    break;
                case NodeType.Statement:
                    Register(keyword, (IStatementParselet)parselet);
                    break;
            }
        }
    }

    static string GetKeyword(SyntaxElement element)
    {
        if (element is KeywordElement keywordElement)
        {
            return keywordElement.Keyword;
        }

        if (element is AndElement and)
        {
            return GetKeyword(and.First);
        }

        return null;
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

        return wrapExpressions ? new ExpressionStatement(expression) : expression;
    }


    public static TranslationUnit Parse<TParser>(string source,
    string filename = "syntethic.dsl",
    bool useToplevelStatements = false)
        where TParser : Parser, new()
    {
        var lexer = new Lexer(source, filename);

        var parser = new TParser { _lexer = lexer };

        AddLexerSymbols(lexer, parser._prefixParselets);
        AddLexerSymbols(lexer, parser._infixParselets);
        AddLexerSymbols(lexer, parser._statementParselets);

        parser.InitLexer(lexer);

        return new(useToplevelStatements
                    ? parser.ParseStatement()
                    : parser.ParseExpression(), lexer.Document);
    }

    private static void AddLexerSymbols<U>(Lexer lexer, Dictionary<Symbol, U> dict)
    {
        foreach (var prefix in dict)
        {
            if (!lexer.ContainsSymbol(prefix.Key.Name) && !prefix.Key.Name.StartsWith("#"))
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
            return new InvalidNode(token);
        }

        var left = prefix.Parse(this, token);

        while (precedence < GetBindingPower())
        {
            token = Consume();

            if (!_infixParselets.TryGetValue(token.Type, out var infix))
            {
                token.Document.Messages.Add(Message.Error("Could not parse \"" + token.Text + "\"."));
            }

            left = infix.Parse(this, left, token);
        }

        return left;
    }

    public AstNode ParseExpression()
    {
        return Parse(0);
    }

    protected abstract void InitLexer(Lexer lexer);


    public List<AstNode> ParseSeperated(Symbol seperator, Symbol terminator, int bindingPower = 0)
    {
        var args = new List<AstNode>();

        if (Match(terminator))
        {
            return [];
        }

        do
        {
            args.Add(Parse(bindingPower));
        } while (Match(seperator));

        Consume(terminator);

        return args;
    }

    public List<AstNode> ParseSeperated(Symbol seperator, Symbol[] terminators, int bindingPower = 0)
    {
        var args = new List<AstNode>();

        if (Match(terminators))
        {
            return [];
        }

        do
        {
            args.Add(Parse(bindingPower));
        } while (Match(seperator));

        Match(terminators);

        return args;
    }

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
        if (!_lexer.ContainsSymbol(expected.Name))
        {
            _lexer.AddSymbol(expected.Name);
        }
    }

    public bool IsMatch(params Symbol[] expected)
    {
        var result = true;
        for (uint i = 0; i < expected.Length; i++)
        {
            result &= IsMatch(expected[i], i);
        }

        return result;
    }

    public Token Consume(Symbol expected)
    {
        var token = LookAhead(0);

        EnsureSymbolIsRegistered(expected);

        if (!token.Type.Equals(expected))
        {
            token.Document.Messages.Add(Message.Error($"Expected token {expected} and found {token.Type}({token})"));
        }

        return Consume();
    }

    public Token Consume()
    {
        // Make sure we've read the token.
        var token = LookAhead(0);
        _read.Remove(token);

        return token;
    }

    public Token[] ConsumeMany(uint count)
    {
        var result = new List<Token>();
        for (var i = 0; i < count; i++)
        {
            result.Add(Consume());
        }

        return result.ToArray();
    }

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
    public void Postfix(Symbol token, int bindingPower)
    {
        Register(token, new PostfixOperatorParselet(bindingPower));
    }


    /// <summary>
    ///     Registers a prefix unary operator parselet for the given token and binding power.
    /// </summary>
    public void Prefix(Symbol token, int bindingPower)
    {
        Register(token, new PrefixOperatorParselet(bindingPower));
    }

    /// <summary>
    ///     Registers a left-associative binary operator parselet for the given token and binding power.
    /// </summary>
    public void InfixLeft(Symbol token, int bindingPower)
    {
        Register(token, new BinaryOperatorParselet(bindingPower, false));
    }

    /// <summary>
    ///     Registers a right-associative binary operator parselet for the given token and binding power.
    /// </summary>
    public void InfixRight(Symbol token, int bindingPower)
    {
        Register(token, new BinaryOperatorParselet(bindingPower, true));
    }

    /// <summary>
    ///     Register a ternary operator like the :? operator
    /// </summary>
    /// <param name="firstSymbol"></param>
    /// <param name="secondSymbol"></param>
    /// <param name="bindingPower"></param>
    public void Ternary(Symbol firstSymbol, Symbol secondSymbol, int bindingPower)
    {
        Register(firstSymbol, new TernaryOperatorParselet(secondSymbol, bindingPower));
    }
}
