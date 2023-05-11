namespace Furesoft.PrattParser;

public enum TokenType {
   LeftParen,
   RightParen,
   Comma,
   Assign,
   Plus,
   Minus,
   Asterisk,
   Slash,
   Caret,
   Tilde,
   Bang,
   Question,
   Colon,
   Name,
   EOF,
}

/// <summary>
/// Methods associated with the <see cref="TokenType"/> enum.
/// </summary>
public static class TokenTypeExtensions {
   /// <summary>
   /// If a <see cref="TokenType"/> represents a punctuator (i.e. a token
   /// that can split an identifier like '+', this will get its text.
   /// </summary>
   public static char Punctuator(this TokenType type) {
      switch (type) {
         case TokenType.LeftParen:
            return '(';

         case TokenType.RightParen:
            return ')';

         case TokenType.Comma:
            return ',';

         case TokenType.Assign:
            return '=';

         case TokenType.Plus:
            return '+';

         case TokenType.Minus:
            return '-';

         case TokenType.Asterisk:
            return '*';

         case TokenType.Slash:
            return '/';

         case TokenType.Caret:
            return '^';

         case TokenType.Tilde:
            return '~';

         case TokenType.Bang:
            return '!';

         case TokenType.Question:
            return '?';

         case TokenType.Colon:
            return ':';

         case TokenType.EOF:
         default:
            return '\0';
      }
   }
}