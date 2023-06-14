using System.Collections.Generic;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using Furesoft.PrattParser.Parselets.Operators;
using Furesoft.PrattParser.Text;

namespace Furesoft.PrattParser;

public class Parser
{
    public static TranslationUnit<T> Parse<T, TParser>(string source, string filename = "syntethic.dsl")
        where TParser : Parser<T>, new()
        where T : class
    {
        return Parser<T>.Parse<TParser>(source, filename);
    }
}

public abstract class Parser<T>
    where T : class
{
    private Lexer _lexer;
    private readonly List<Token> _read = new();
    private readonly Dictionary<Symbol, IPrefixParselet<T>> _prefixParselets = new();
    private readonly Dictionary<Symbol, IInfixParselet<T>> _infixParselets = new();

    public void Register(Symbol token, IPrefixParselet<T> parselet)
    {
        _prefixParselets.Add(token, parselet);
    }

    public void Register(Symbol token, IInfixParselet<T> parselet)
    {
        _infixParselets.Add(token, parselet);
    }

    public void Register(IInfixParselet<T> parselet, params Symbol[] tokens)
    {
        foreach (var token in tokens)
        {
            Register(token, parselet);
        }
    }
    
    public void Register(IPrefixParselet<T> parselet, params Symbol[] tokens)
    {
        foreach (var token in tokens)
        {
            Register(token, parselet);
        }
    }

    public void Group(Symbol leftToken, Symbol rightToken)
    {
        Register(leftToken, (IPrefixParselet<T>)new GroupParselet(rightToken));
    }

    public void Block(Symbol seperator, Symbol terminator, int bindingPower = 500)
    {
        Register(seperator, (IInfixParselet<T>)new BlockParselet(seperator, terminator, bindingPower));
    }
    
    public static TranslationUnit<T> Parse<TParser>(string source, string filename = "syntethic.dsl") 
        where TParser : Parser<T>, new()
    {
        var lexer = new Lexer(source, filename);

        var parser = new TParser
        {
            _lexer = lexer
        };

        AddLexerSymbols(lexer, parser._prefixParselets);
        AddLexerSymbols(lexer, parser._infixParselets);

        parser.InitLexer(lexer);

        return new(parser.Parse(), lexer.Document);
    }

    private static void AddLexerSymbols<U>(Lexer lexer, Dictionary<Symbol, U> dict)
    {
        foreach (var prefix in dict)
        {
            if (!lexer.ContainsSymbol(prefix.Key.Name))
            {
                lexer.AddSymbol(prefix.Key.Name);
            }
        }
    }

    public T Parse(int precedence)
    {
        var token = Consume();

        if (!_prefixParselets.TryGetValue(token.Type, out var prefix))
        {
            token.Document.Messages.Add(Message.Error("Could not parse prefix \"" + token.Text + "\".", token.GetRange()));
            return new InvalidNode(token) as T;
        }

        var left = prefix.Parse(this, token);

        while (precedence < GetBindingPower())
        {
            token = Consume();

            if (!_infixParselets.TryGetValue(token.Type, out var infix))
            {
                token.Document.Messages.Add(Message.Error(("Could not parse \"" + token.Text + "\".")));
            }

            left = infix.Parse(this, left, token);
        }

        return left;
    }

    public T Parse()
    {
        return Parse(0);
    }

    protected abstract void InitLexer(Lexer lexer);
    
    public List<T> ParseSeperated(Symbol seperator, Symbol terminator, int bindingPower = 0)
    {
        var args = new List<T>();

        if (Match(terminator))
        {
            return new();
        }

        do
        {
            args.Add(Parse(bindingPower));
        } while (Match(seperator));

        Consume(terminator);

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
        for (int i = 0; i < count; i++)
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

    /// <summary>
    /// Register a ternary operator like the :? operator
    /// </summary>
    /// <param name="firstSymbol"></param>
    /// <param name="secondSymbol"></param>
    /// <param name="bindingPower"></param>
    public void Ternary(Symbol firstSymbol, Symbol secondSymbol, int bindingPower)
    {
        Register(firstSymbol, (IInfixParselet<T>)new TernaryOperatorParselet(secondSymbol, bindingPower));
    }
}
