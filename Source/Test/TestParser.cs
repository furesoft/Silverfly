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
      
      Register("=", new AssignParselet());
      Register("?", new ConditionalParselet());
      Group("(", ")");
      Register(")", new CallParselet());

      // Register the simple operator parselets.
      Prefix("+", BindingPower.Prefix);
      Prefix("-", BindingPower.Prefix);
      Prefix("~", BindingPower.Prefix);
      Prefix("1", BindingPower.Prefix);

      // For kicks, we'll make "!" both prefix and postfix, kind of like ++.
      Postfix("!", BindingPower.PostFix);

      InfixLeft("+", BindingPower.Sum);
      InfixLeft("-", BindingPower.Sum);
      InfixLeft("*", BindingPower.Product);
      InfixLeft("/", BindingPower.Product);
      InfixRight("^", BindingPower.Exponent);
   }
}
