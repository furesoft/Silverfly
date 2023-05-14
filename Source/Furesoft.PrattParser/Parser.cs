using System.Collections.Generic;
using Furesoft.PrattParser.Parselets;

namespace Furesoft.PrattParser;

public class Parser<T, TokenType> {
   private ILexer<TokenType> _lexer;
   private List<Token<TokenType>> _read = new();
   private Dictionary<TokenType, IPrefixParselet<T, TokenType>> _prefixParselets = new();
   private Dictionary<TokenType, IInfixParselet<T, TokenType>> _infixParselets = new();

   public Parser(ILexer<TokenType> lexer) {
      _lexer = lexer;
   }

   public void Register(TokenType token, IPrefixParselet<T, TokenType> parselet) {
      _prefixParselets.Add(token, parselet);
   }

   public void Register(TokenType token, IInfixParselet<T, TokenType> parselet) {
      _infixParselets.Add(token, parselet);
   }
      
   public void Group(TokenType leftToken, TokenType rightToken)
   {
      Register(leftToken, (IPrefixParselet<T, TokenType>)new GroupParselet<TokenType>(rightToken));
   }

   public T Parse(int precedence) {
      var token = Consume();

      if (!_prefixParselets.TryGetValue(token.Type, out var prefix)) {
         throw new ParseException("Could not parse \"" + token.Text + "\".");
      }

      var left = prefix.Parse(this, token);

      while (precedence < GetBindingPower()) {
         token = Consume();

         if (!_infixParselets.TryGetValue(token.Type, out var infix)) {
            throw new ParseException("Could not parse \"" + token.Text + "\".");
         }
         left = infix.Parse(this, left, token);
      }

      return left;
   }

   public T Parse() {
      return Parse(0);
   }

   public bool Match(TokenType expected) {
      var token = LookAhead(0);
      
      if (!token.Type.Equals(expected)) {
         return false;
      }

      Consume();
      return true;
   }

   public Token<TokenType> Consume(TokenType expected) {
      var token = LookAhead(0);
      if (!token.Type.Equals(expected)) {
         throw new ParseException("Expected token " + expected + " and found " + token.Type);
      }

      return Consume();
   }

   public Token<TokenType> Consume() {
      // Make sure we've read the token.
      var token = LookAhead(0);
      _read.Remove(token);
      
      return token;
   }

   private Token<TokenType> LookAhead(int distance) {
      // Read in as many as needed.
      while (distance >= _read.Count) {
         _read.Add(_lexer.Next());
      }

      // Get the queued token.
      return _read[distance];
   }

   private int GetBindingPower() {
      if (_infixParselets.TryGetValue(LookAhead(0).Type, out var parselet)) {
         return parselet.GetBindingPower();
      }
      
      return 0;
   }
}
