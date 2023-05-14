namespace Furesoft.PrattParser;

/// <summary>
/// Methods associated with the <see cref="Symbol"/> enum.
/// </summary>
public static class SymbolExtensions {
   /// <summary>
   /// If a <see cref="Symbol"/> represents a punctuator (i.e. a token
   /// that can split an identifier like '+', this will get its text.
   /// </summary>
   public static string Punctuator(this Symbol type) {
       if (type == PredefinedSymbols.Name || type == PredefinedSymbols.EOF)
       {
           return "\0";
       }

       return type.Name;
   }
}
