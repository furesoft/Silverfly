using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Simple parselet for a named variable like "abc".
/// </summary>
public class NameParselet : IPrefixParselet<IExpression, TokenType> {
   public IExpression Parse(Parser<IExpression, TokenType> parser, Token<TokenType> token) {
      return new NameExpression(token.Text);
   }
}
