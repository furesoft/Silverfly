using Furesoft.PrattParser;
using Furesoft.PrattParser.Expressions;
using Furesoft.PrattParser.Parselets;

namespace Test;

public class TestParser : Parser<IExpression> {
   public TestParser(Lexer lexer) : base(lexer) 
   { 
       Register(PredefinedSymbols.Name, new NameParselet());
      
      Register("=", new AssignParselet());
      Register("?", new ConditionalParselet());
      Group("(", ")");
      Register(")", new CallParselet());
      
      Prefix("+", BindingPower.Prefix);
      Prefix("-", BindingPower.Prefix);
      Prefix("~", BindingPower.Prefix);
      Prefix("1", BindingPower.Prefix);

      Postfix("!", BindingPower.PostFix);

      InfixLeft("+", BindingPower.Sum);
      InfixLeft("-", BindingPower.Sum);
      InfixLeft("*", BindingPower.Product);
      InfixLeft("/", BindingPower.Product);
      InfixRight("^", BindingPower.Exponent);
   }
}
