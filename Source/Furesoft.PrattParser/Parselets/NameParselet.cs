using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Simple parselet for a named variable like "abc".
/// </summary>
public class NameParselet : IPrefixParselet<IExpression> {
   public IExpression Parse(Parser<IExpression> parser, Token token) {
      return new NameExpression(token.Text);
   }
}