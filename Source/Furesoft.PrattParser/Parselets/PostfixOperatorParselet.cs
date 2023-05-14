using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Generic infix parselet for an unary arithmetic operator. Parses
/// postfix unary "?" expressions.
/// </summary>
public class PostfixOperatorParselet<TokenType> : IInfixParselet<IExpression, TokenType> {
   private readonly int _bindingPower;

   public PostfixOperatorParselet(int bindingPower) {
      _bindingPower = bindingPower;
   }

   public IExpression Parse(Parser<IExpression, TokenType> parser, IExpression left, Token<TokenType> token) {
      return new PostfixExpression<TokenType>(left, token.Type);
   }

   public int GetBindingPower() {
      return _bindingPower;
   }
}
