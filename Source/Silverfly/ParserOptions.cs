namespace Silverfly;

/// <summary>
///     Represents options for a test scenario.
/// </summary>
public record ParserOptions(bool UseStatementsAtToplevel, bool EnforceEndOfFile = false)
{
}
