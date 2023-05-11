using Furesoft.PrattParser;
using Furesoft.PrattParser.Expressions;
using Furesoft.PrattParser.Parselets;

namespace Test;

/// <summary>
/// Extends the generic Parser class with support for parsing.
/// </summary>
public class TestParser : Parser<IExpression> {
   public TestParser(Lexer lexer) : base(lexer) {

      // Register all of the parselets for the grammar.

      // Register the ones that need special parselets.
      Register(TokenType.Name, new NameParselet());
      Register(TokenType.Assign, new AssignParselet());
      Register(TokenType.Question, new ConditionalParselet());
      Group(TokenType.LeftParen, TokenType.RightParen);
      Register(TokenType.LeftParen, new CallParselet());

      // Register the simple operator parselets.
      Prefix(TokenType.Plus, BindingPower.Prefix);
      Prefix(TokenType.Minus, BindingPower.Prefix);
      Prefix(TokenType.Tilde, BindingPower.Prefix);
      Prefix(TokenType.Bang, BindingPower.Prefix);

      // For kicks, we'll make "!" both prefix and postfix, kind of like ++.
      Postfix(TokenType.Bang, BindingPower.PostFix);

      InfixLeft(TokenType.Plus, BindingPower.Sum);
      InfixLeft(TokenType.Minus, BindingPower.Sum);
      InfixLeft(TokenType.Asterisk, BindingPower.Product);
      InfixLeft(TokenType.Slash, BindingPower.Product);
      InfixRight(TokenType.Caret, BindingPower.Exponent);
   }

      

   /// <summary>
   /// Registers a postfix unary operator parselet for the given token and binding power.
   /// </summary>
   public void Postfix(TokenType token, int bindingPower) {
      Register(token, new PostfixOperatorParselet(bindingPower));
   }

   /// <summary>
   /// Registers a prefix unary operator parselet for the given token and binding power.
   /// </summary>
   public void Prefix(TokenType token, int bindingPower) {
      Register(token, new PrefixOperatorParselet(bindingPower));
   }

   /// <summary>
   ///  Registers a left-associative binary operator parselet for the given token and binding power.
   /// </summary>
   public void InfixLeft(TokenType token, int bindingPower) {
      Register(token, new BinaryOperatorParselet(bindingPower, false));
   }

   /// <summary>
   /// Registers a right-associative binary operator parselet for the given token and binding power.
   /// </summary>
   public void InfixRight(TokenType token, int bindingPower) {
      Register(token, new BinaryOperatorParselet(bindingPower, true));
   }
}