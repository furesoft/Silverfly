using System;
using System.Collections.Generic;

namespace Furesoft.PrattParser.Text;

public class SourceDocument
{
    public string Filename { get; set; }
    public ReadOnlyMemory<char> Source { get; set; }
    public List<Message> Messages { get; } = [];
}
