using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parses parentheses used to group an expression, like "(b + c)".
/// </summary>
public class GroupParselet : IPrefixParselet<IExpression> {
   private readonly TokenType _rightTokenType;

   public GroupParselet(TokenType rightTokenType)
   {
      _rightTokenType = rightTokenType;
   }
      
   public IExpression Parse(Parser<IExpression> parser, Token token) {
      var expression = parser.Parse();
      parser.Consume(_rightTokenType);
         
      return expression;
   }
}