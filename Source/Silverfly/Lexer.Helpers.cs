using System;
using Silverfly.Text;

namespace Silverfly;

public partial class Lexer
{
    /// <summary>
    /// Checks if the current position in the document matches the specified token. If it matches, advances the position; otherwise, adds an error message to the document.
    /// </summary>
    /// <param name="token">The token to match against the current position.</param>
    /// <param name="ignoreCase">Whether to ignore case when matching the token. Default is <c>false</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="token"/> is <c>null</c>.</exception>
    public void Expect(Symbol token, bool ignoreCase = false)
    {
        var span = new SourceSpan(_line, _column);
        if (IsMatch(token, ignoreCase))
        {
            Advance(token.Name.Length);
            return;
        }

        Document.AddMessage(MessageSeverity.Error,
            $"Expected '{token}'", new SourceRange(Document, span, span));
    }
}
