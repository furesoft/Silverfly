namespace Silverfly.Testing;

/// <summary>
/// Represents options for a test scenario.
/// </summary>
public record TestOptions(string Filename = "test.src", OutputMode OutputMode = OutputMode.Small)
{
}
