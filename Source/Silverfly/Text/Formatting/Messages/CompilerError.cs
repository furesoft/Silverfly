using System.Collections.Generic;

namespace Silverfly.Text.Formatting.Messages;

internal class CompilerError(Message message)
{
    public Message Message { get; } = message;
    public List<string> SourceLines { get; set; } = [];
    public List<int> HighlightLines { get; set; } = [];

    public static CompilerError FromMessage(Message message)
    {
        var error = new CompilerError(message);
        error.HighlightLines.Add(message.Range.Start.Line);
        error.SourceLines.Add(message.Range.GetLineText());

        return error;
    }
}
