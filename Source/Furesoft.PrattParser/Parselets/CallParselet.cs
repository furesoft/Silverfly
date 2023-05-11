using System.Collections.Generic;
using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Parselet to parse a function call like "a(b, c, d)".
/// </summary>
public class CallParselet : IInfixParselet<IExpression> {
   public IExpression Parse(Parser<IExpression> parser, IExpression left, Token token) {
      // Parse the comma-separated arguments until we hit, ')'.
      List<IExpression> args = new List<IExpression>();

      // There may be no arguments at all.
      if (!parser.Match(TokenType.RightParen)) {
         do {
            args.Add(parser.Parse());
         } while (parser.Match(TokenType.Comma));
         parser.Consume(TokenType.RightParen);
      }

      return new CallExpression(left, args);
   }

   public int GetBindingPower() {
      return BindingPower.Call;
   }
}