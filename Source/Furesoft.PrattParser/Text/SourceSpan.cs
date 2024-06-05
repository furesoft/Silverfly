namespace Furesoft.PrattParser.Text;

/// <summary>Represents a part of the source file</summary>
public readonly struct SourceSpan(int line, int column)
{
    public int Line { get; } = line;

    public int Column { get; } = column;

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
