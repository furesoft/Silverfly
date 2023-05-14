using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parselet for the condition or "ternary" operator, like "a ? b : c".
/// </summary>
public class ConditionalParselet : IInfixParselet<IExpression> {
   public IExpression Parse(Parser<IExpression> parser, IExpression left, Token token) {
      var thenArm = parser.Parse();
      parser.Consume(PredefinedSymbols.Colon);
      
      var elseArm = parser.Parse(BindingPower.Conditional - 1);

      return new ConditionalExpression(left, thenArm, elseArm);
   }

   public int GetBindingPower() {
      return BindingPower.Conditional;
   }
}
