using System;
using System.Collections.Generic;

namespace Furesoft.PrattParser;

/// <summary>
/// A very primitive lexer. Takes a string and splits it into a series of Tokens.
/// Operators and punctuation are mapped to unique keywords. Names, which can be
/// any series of letters, are turned into NAME tokens. All other characters are
/// ignored (except to separate names). Numbers and strings are not supported. This
/// is really just the bare minimum to give the parser something to work with.
/// </summary>
public class Lexer : ILexer {
   private readonly Dictionary<string, Symbol> _punctuators;
   private readonly string _source;
   private int _index;

   /// <summary>
   /// Creates a new <see cref="Lexer"/> to tokenize the given string.
   /// </summary>
   /// <param name="text">String to tokenize</param>
   public Lexer(string text) {
      _punctuators = new();
      _index = 0;
      _source = text;

      // Register all of the Symbols that are explicit punctuators.
      foreach(var type in (Symbol[])Enum.GetValues(typeof(Symbol))) {
         var punctuator = type.Punctuator();
         if (punctuator != "\0") _punctuators.Add(punctuator, type);
      }
   }

   public Token Next() {
      while (_index < _source.Length) {
         var c = _source[_index++];

         if (_punctuators.TryGetValue(c.ToString(), out var Symbol)) {
            return new(Symbol, char.ToString(c));
         }
         else if (char.IsLetter(c)) {
            // Handle names.
            var start = _index - 1;
            while (_index < _source.Length) {
               if (!char.IsLetter(_source[_index])) break;
               _index++;
            }

            var name = _source.Substring(start, _index- start);
            return new(PredefinedSymbols.Name, name);
         }
         else {
            // Ignore all other characters (whitespace, etc.)
         }
      }

      // Once we've reached the end of the string, just return EOF tokens. We'll
      // just keeping returning them as many times as we're asked so that the
      // parser's lookahead doesn't have to worry about running out of tokens.
      return new(PredefinedSymbols.EOF, string.Empty);
   }
}
