using Furesoft.PrattParser.Expressions;

namespace Furesoft.PrattParser.Parselets;

/// <summary>
/// Simple parselet for a named variable like "abc".
/// </summary>
public class NameParselet : IPrefixParselet<IAstNode> {
   public IAstNode Parse(Parser<IAstNode> parser, Token token) {
      return new NameAstNode(token.Text);
   }
}
