using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parses parentheses used to group an expression, like "(b + c)".
/// </summary>
public class GroupParselet : IPrefixParselet<IExpression> {
   private readonly Symbol _rightSymbol;

   public GroupParselet(Symbol rightSymbol)
   {
      _rightSymbol = rightSymbol;
   }
      
   public IExpression Parse(Parser<IExpression> parser, Token token) {
      var expression = parser.Parse();
      parser.Consume(_rightSymbol);
         
      return expression;
   }
}
