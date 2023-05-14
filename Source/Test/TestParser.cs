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
      Register(PredefinedSymbols.Name, new NameParselet());
      Register(PredefinedSymbols.Equals, new AssignParselet());
      Register(PredefinedSymbols.Question, new ConditionalParselet());
      Group(PredefinedSymbols.LeftParen, PredefinedSymbols.RightParen);
      Register(PredefinedSymbols.LeftParen, new CallParselet());

      // Register the simple operator parselets.
      Prefix(PredefinedSymbols.Plus, BindingPower.Prefix);
      Prefix(PredefinedSymbols.Minus, BindingPower.Prefix);
      Prefix(PredefinedSymbols.Tilde, BindingPower.Prefix);
      Prefix(PredefinedSymbols.Bang, BindingPower.Prefix);

      // For kicks, we'll make "!" both prefix and postfix, kind of like ++.
      Postfix(PredefinedSymbols.Bang, BindingPower.PostFix);

      InfixLeft(PredefinedSymbols.Plus, BindingPower.Sum);
      InfixLeft(PredefinedSymbols.Minus, BindingPower.Sum);
      InfixLeft(PredefinedSymbols.Asterisk, BindingPower.Product);
      InfixLeft(PredefinedSymbols.Slash, BindingPower.Product);
      InfixRight(PredefinedSymbols.Caret, BindingPower.Exponent);
   }
}
