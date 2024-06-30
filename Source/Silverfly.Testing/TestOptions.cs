namespace Silverfly.Testing;

public enum OutputMode
{
    Small,
    Full
}

public record TestOptions(bool UseStatementsAtToplevel, string Filename = "test.src", OutputMode OutputMode = OutputMode.Small)
{
}
