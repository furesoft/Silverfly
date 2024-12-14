namespace Silverfly.Sample.Func.Values;

public record RangeValue : Value
{
    public int Start { get; set; }
    public int End { get; set; }

    public static RangeValue Create(Value start, Value end)
    {
        var range = new RangeValue { Start = (int)((NumberValue)start).Value, End = (int)((NumberValue)end).Value };

        range.Members.Define("start", start);
        range.Members.Define("end", end);

        return range;
    }

    public override bool IsTruthy()
    {
        return true;
    }

    public override string ToString()
    {
        return $"{Start}..{End}";
    }
}
