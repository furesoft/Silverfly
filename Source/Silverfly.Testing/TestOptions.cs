namespace Silverfly.Testing;

/// <summary>
/// Represents options for a test scenario.
/// </summary>
public record TestOptions(bool UseStatementsAtToplevel, string Filename = "test.src",
    OutputMode OutputMode = OutputMode.Small, bool EnforceEndOfFile)
{
}
