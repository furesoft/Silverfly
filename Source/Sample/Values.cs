namespace Sample;

public abstract record Value();

public record NumberValue(double Value) : Value;

public record FuncValue(Delegate Value) : Value;

public record UnitValue() : Value;

public record BoolValue(bool Value) : Value;