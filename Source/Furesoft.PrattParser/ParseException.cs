using System;

namespace Furesoft.PrattParser;

public class ParseException : Exception
{
    public ParseException(string message) : base(message) { }
}
