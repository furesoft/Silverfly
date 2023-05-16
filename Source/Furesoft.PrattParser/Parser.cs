using System.Collections.Generic;
using Furesoft.PrattParser.Parselets;

namespace Furesoft.PrattParser;

public class Parser<T>
{
    private ILexer _lexer;
    private List<Token> _read = new();
    private Dictionary<Symbol, IPrefixParselet<T>> _prefixParselets = new();
    private Dictionary<Symbol, IInfixParselet<T>> _infixParselets = new();

    public Parser(ILexer lexer)
    {
        _lexer = lexer;
    }

    public void Register(Symbol token, IPrefixParselet<T> parselet)
    {
        _prefixParselets.Add(token, parselet);
    }

    public void Register(string token, IPrefixParselet<T> parselet)
    {
        _prefixParselets.Add(PredefinedSymbols.Pool.Get(token), parselet);
    }

    public void Register(Symbol token, IInfixParselet<T> parselet)
    {
        _infixParselets.Add(token, parselet);
    }

    public void Register(string token, IInfixParselet<T> parselet)
    {
        _infixParselets.Add(PredefinedSymbols.Pool.Get(token), parselet);
    }

    public void Group(Symbol leftToken, Symbol rightToken)
    {
        Register(leftToken, (IPrefixParselet<T>)new GroupParselet(rightToken));
    }

    public void Group(string left, string right)
    {
        Group(PredefinedSymbols.Pool.Get(left), PredefinedSymbols.Pool.Get(right));
    }

    public T Parse(int precedence)
    {
        var token = Consume();

        if (!_prefixParselets.TryGetValue(token.Type, out var prefix))
        {
            throw new ParseException("Could not parse \"" + token.Text + "\".");
        }

        var left = prefix.Parse(this, token);

        while (precedence < GetBindingPower())
        {
            token = Consume();

            if (!_infixParselets.TryGetValue(token.Type, out var infix))
            {
                throw new ParseException("Could not parse \"" + token.Text + "\".");
            }

            left = infix.Parse(this, left, token);
        }

        return left;
    }

    public T Parse()
    {
        return Parse(0);
    }

    public List<T> ParseSeperated(Symbol seperator, Symbol terminator)
    {
        var args = new List<T>();

        if (Match(terminator))
        {
            return new();
        }

        do
        {
            args.Add(Parse());
        } while (Match(seperator));

        Consume(terminator);

        return args;
    }

    public bool Match(Symbol expected)
    {
        var token = LookAhead(0);

        if (!token.Type.Equals(expected))
        {
            return false;
        }

        Consume();
        return true;
    }

    public Token Consume(Symbol expected)
    {
        var token = LookAhead(0);

        if (!token.Type.Equals(expected))
        {
            throw new ParseException($"Expected token {expected} and found {token.Type}({token})");
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

    private Token LookAhead(int distance)
    {
        // Read in as many as needed.
        while (distance >= _read.Count)
        {
            _read.Add(_lexer.Next());
        }

        // Get the queued token.
        return _read[distance];
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
    /// Registers a postfix unary operator parselet for the given token and binding power.
    /// </summary>
    public void Postfix(Symbol token, int bindingPower)
    {
        Register(token, (IInfixParselet<T>)new PostfixOperatorParselet(bindingPower));
    }
    

    /// <summary>
    /// Registers a prefix unary operator parselet for the given token and binding power.
    /// </summary>
    public void Prefix(Symbol token, int bindingPower)
    {
        Register(token, (IPrefixParselet<T>)new PrefixOperatorParselet(bindingPower));
    }

    /// <summary>
    ///  Registers a left-associative binary operator parselet for the given token and binding power.
    /// </summary>
    public void InfixLeft(Symbol token, int bindingPower)
    {
        Register(token, (IInfixParselet<T>)new BinaryOperatorParselet(bindingPower, false));
    }

    /// <summary>
    /// Registers a right-associative binary operator parselet for the given token and binding power.
    /// </summary>
    public void InfixRight(Symbol token, int bindingPower)
    {
        Register(token, (IInfixParselet<T>)new BinaryOperatorParselet(bindingPower, true));
    }
}
