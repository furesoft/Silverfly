using Silverfly.Text;

namespace Silverfly;

public partial class Lexer
{
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
