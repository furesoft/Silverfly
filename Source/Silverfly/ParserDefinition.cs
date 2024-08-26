using System.Collections.Generic;
using Silverfly.Parselets;
using Silverfly.Parselets.Operators;

namespace Silverfly;

public partial class ParserDefinition
{
    internal readonly Dictionary<Symbol, IInfixParselet> _infixParselets = [];
    internal readonly Dictionary<Symbol, IPrefixParselet> _prefixParselets = [];
    internal readonly Dictionary<Symbol, IStatementParselet> _statementParselets = [];
    public PrecedenceLevels PrecedenceLevels = new DefaultPrecedenceLevels();
    
    /// <summary>
    /// Registers a prefix parselet for the given token.
    /// </summary>
    /// <param name="token">The symbol token.</param>
    /// <param name="parselet">The prefix parselet to be registered.</param>
    public void Register(Symbol token, IPrefixParselet parselet)
    {
        _prefixParselets[token] = parselet;
    }

    /// <summary>
    /// Registers an infix parselet for the given token.
    /// </summary>
    /// <param name="token">The symbol token.</param>
    /// <param name="parselet">The infix parselet to be registered.</param>
    public void Register(Symbol token, IInfixParselet parselet)
    {
        _infixParselets[token] = parselet;
    }

    /// <summary>
    /// Registers a statement parselet for the given token.
    /// </summary>
    /// <param name="token">The symbol token.</param>
    /// <param name="parselet">The statement parselet to be registered.</param>
    public void Register(Symbol token, IStatementParselet parselet)
    {
        _statementParselets[token] = parselet;
    }

    /// <summary>
    /// Registers an infix parselet for multiple tokens.
    /// </summary>
    /// <param name="parselet">The infix parselet to be registered.</param>
    /// <param name="tokens">The array of symbol tokens.</param>
    public void Register(IInfixParselet parselet, params Symbol[] tokens)
    {
        foreach (var token in tokens)
        {
            Register(token, parselet);
        }
    }

    /// <summary>
    /// Registers a prefix parselet for multiple tokens.
    /// </summary>
    /// <param name="parselet">The prefix parselet to be registered.</param>
    /// <param name="tokens">The array of symbol tokens.</param>
    public void Register(IPrefixParselet parselet, params Symbol[] tokens)
    {
        foreach (var token in tokens)
        {
            Register(token, parselet);
        }
    }

    /// <summary>
    /// Registers a prefix parselet for multiple tokens.
    /// </summary>
    /// <param name="parselet">The prefix parselet to be registered.</param>
    /// <param name="tokens">The array of symbol tokens.</param>
    public void Register(IPrefixParselet parselet, params string[] tokens)
    {
        foreach (var token in tokens)
        {
            Register(token, parselet);
        }
    }

    /// <summary>
    /// Registers a group parselet for the given left and right tokens.
    /// </summary>
    /// <param name="leftToken">The left symbol token.</param>
    /// <param name="rightToken">The right symbol token.</param>
    /// <param name="tag">An optional tag symbol.</param>
    public void Group(Symbol leftToken, Symbol rightToken, Symbol tag = null)
    {
        Register(leftToken, new GroupParselet(leftToken, rightToken, tag));
    }

    /// <summary>
    /// Registers a block parselet for the given start and terminator tokens.
    /// </summary>
    /// <param name="start">The start symbol token.</param>
    /// <param name="terminator">The terminator symbol token.</param>
    /// <param name="separator">An optional separator symbol.</param>
    /// <param name="wrapExpressions">Indicates whether to wrap expressions.</param>
    /// <param name="tag">An optional tag symbol.</param>
    public void Block(Symbol start, Symbol terminator, Symbol separator = null, bool wrapExpressions = false,
        Symbol tag = null)
    {
        Register(start, new BlockParselet(terminator, separator, wrapExpressions, tag));
    }
    
    /// <summary>
    /// Registers a postfix unary operator parselet for the given token and binding power.
    /// </summary>
    /// <param name="token">The symbol token.</param>
    /// <param name="bindingPowerName">The name of the binding power. Default is "Postfix".</param>
    public void Postfix(Symbol token, string bindingPowerName = "Postfix", string tag = null)
    {
        Register(token,
            new PostfixOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName), tag));
    }

    /// <summary>
    /// Registers a prefix unary operator parselet for the given token and binding power.
    /// </summary>
    /// <param name="token">The symbol token.</param>
    /// <param name="bindingPowerName">The name of the binding power. Default is "Prefix".</param>
    public void Prefix(Symbol token, string bindingPowerName = "Prefix", string tag = null)
    {
        Register(token,
            new PrefixOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName), tag));
    }

    /// <summary>
    /// Registers a left-associative binary operator parselet for the given token and binding power.
    /// </summary>
    /// <param name="token">The symbol token.</param>
    /// <param name="bindingPowerName">The name of the binding power.</param>
    public void InfixLeft(Symbol token, string bindingPowerName)
    {
        Register(token, new BinaryOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName), false));
    }

    /// <summary>
    /// Registers a right-associative binary operator parselet for the given token and binding power.
    /// </summary>
    /// <param name="token">The symbol token.</param>
    /// <param name="bindingPowerName">The name of the binding power.</param>
    public void InfixRight(Symbol token, string bindingPowerName)
    {
        Register(token, new BinaryOperatorParselet(PrecedenceLevels.GetPrecedence(bindingPowerName), true));
    }

    /// <summary>
    /// Registers a ternary operator like the ?: operator.
    /// </summary>
    /// <param name="firstSymbol">The first symbol token.</param>
    /// <param name="secondSymbol">The second symbol token.</param>
    /// <param name="bindingPowerName">The name of the binding power.</param>
    public void Ternary(Symbol firstSymbol, Symbol secondSymbol, string bindingPowerName)
    {
        Register(firstSymbol,
            new TernaryOperatorParselet(secondSymbol, PrecedenceLevels.GetPrecedence(bindingPowerName)));
    }
}
