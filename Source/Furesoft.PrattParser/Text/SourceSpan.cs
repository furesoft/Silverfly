namespace Furesoft.PrattParser.Text;

public readonly struct SourceSpan
{
    public SourceSpan(int line, int column)
    {
        Line = line;
        Column = column;
    }

    public int Line { get; }

    public int Column { get; }

    public static bool operator <(SourceSpan left, SourceSpan right)
    {
        if (left.Line < right.Line)
        {
            return true;
        }

        return left.Line == right.Line && 
               left.Column < right.Column;
    }
    
    public static bool operator >(SourceSpan left, SourceSpan right)
    {
        return !(left < right);
    }

    public override string ToString()
    {
        return $"{Line}:{Column}";
    }
}
