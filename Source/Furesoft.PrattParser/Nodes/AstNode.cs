using System.Collections.Generic;
using System.Linq;

namespace Furesoft.PrattParser.Nodes;

public abstract class AstNode
{
    public virtual SourceRange Range => CalculateNodeRange();

    // ToDo: Test Automatic SourceRange Calculation
    private SourceRange CalculateNodeRange()
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
        return GetType()
            .GetProperties()
            .Where(_ => _.PropertyType == typeof(SourceRange))
            .Select(_=> (SourceRange)_.GetValue(this));
    }

    public T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
