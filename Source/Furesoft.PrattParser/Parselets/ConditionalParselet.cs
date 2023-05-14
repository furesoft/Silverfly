using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parselet for the condition or "ternary" operator, like "a ? b : c".
/// </summary>
public class ConditionalParselet : IInfixParselet<IExpression, TokenType> {
   public IExpression Parse(Parser<IExpression, TokenType> parser, IExpression left, Token<TokenType> token) {
      var thenArm = parser.Parse();
      parser.Consume(TokenType.Colon);
      
      var elseArm = parser.Parse(BindingPower.Conditional - 1);

      return new ConditionalExpression(left, thenArm, elseArm);
   }

   public int GetBindingPower() {
      return BindingPower.Conditional;
   }
}
