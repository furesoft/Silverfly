namespace Sample;

public abstract record Value();

public record NumberValue(double value) : Value;

public record FuncValue(Delegate value) : Value;

public record UnitValue() : Value;

public record BoolValue(bool value) : Value;