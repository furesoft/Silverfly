using Silverfly.Nodes;

namespace Silverfly.Parselets;

/// <summary>
///     One of the parselet interfaces used by the Pratt parser. An
///     <see cref="IInfixParselet" /> is associated with a token that appears in the
///     middle of the expression it parses. Its Parse() method will be called after the
///     left-hand side has been parsed, and it in turn is responsible for parsing
///     everything that comes after the token.This is also used for postfix expressions,
///     in which case it simply doesn't consume any more tokens in its Parse() call.
///     See <see cref="IPrefixParselet" />.
/// </summary>
public interface IInfixParselet
{
    AstNode Parse(Parser parser, AstNode left, Token token);
    int GetBindingPower();
}
