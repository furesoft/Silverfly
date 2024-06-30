using Silverfly.Nodes;

namespace Silverfly.Parselets;

/// <summary>
/// One of the two interfaces used by the Pratt parser. <see cref="IPrefixParselet"/> 
/// is associated with a token that appears at the beginning of an expression. Its Parse()
/// method will be called with the consumed leading token, and the parselet is responsible
/// for parsing anything that comes after that token. This interface is also used for
/// single-token expressions like variables, in which case parse() simply doesn't consume
/// any more tokens.
/// 
/// See <see cref="IInfixParselet"/>.
/// </summary>
public interface IPrefixParselet
{
    AstNode Parse(Parser parser, Token token);
}
