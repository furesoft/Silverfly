using System;

namespace Silverfly.Lexing;

public class LexerContext<TContext>(Action<ILexerContext> setContext) : IDisposable
    where TContext : ILexerContext, new()
{
    public void Dispose()
    {
        setContext(null);
    }
}
