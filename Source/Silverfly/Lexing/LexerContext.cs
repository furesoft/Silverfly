using System;

namespace Silverfly.Lexing;

public class LexerContext(ILexerContext oldContext, Action<ILexerContext> setContext) : IDisposable
{
    public void Dispose()
    {
        setContext(oldContext);
    }
}
