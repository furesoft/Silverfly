using System.Collections.Generic;
using System.Linq;

namespace Furesoft.PrattParser.Nodes;

public abstract class AstNode
{
    public virtual SourceRange Range { get; set; }

    // ToDo: Test Automatic SourceRange Calculation
    protected SourceRange CalculateNodeRange()
    {
        var sourceRanges = GetSourceRanges();

        SourceSpan minimumSpan = new(), maximumSpan = new();

        // find lowest source range
        foreach (var sourceRange in sourceRanges)
        {
            if (sourceRange.Start < minimumSpan)
            {
                minimumSpan = sourceRange.Start;
            }
        }

        // find highest source range
        foreach (var sourceRange in sourceRanges)
        {
            if (sourceRange.End > maximumSpan)
            {
                maximumSpan = sourceRange.End;
            }
        }

        return new(minimumSpan, maximumSpan);
    }

    private IEnumerable<SourceRange> GetSourceRanges()
    {
        var properties = GetType().GetProperties();
        var rangeProperties = properties.Where(_ => _.PropertyType == typeof(SourceRange)).ToArray();

        return rangeProperties.Select(_ => (SourceRange)_.GetValue(this));
    }
    
    public AstNode WithRange(Token token)
    {
        Range = new(token.GetSourceSpanStart(), token.GetSourceSpanEnd());
        
        return this;
    }
    
    public AstNode WithRange(SourceSpan start, SourceSpan end)
    {
        Range = new(start, end);
        
        return this;
    }
    
    public AstNode WithRange(Token start, Token end)
    {
        Range = new(start.GetSourceSpanStart(), end.GetSourceSpanEnd());
        
        return this;
    }

    public T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
