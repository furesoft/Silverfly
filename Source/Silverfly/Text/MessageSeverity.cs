namespace Silverfly.Text;

/// <summary>
/// Represents the severity level of a message.
/// </summary>
public enum MessageSeverity
{
    /// <summary>
    /// Indicates an error message.
    /// </summary>
    Error,

    /// <summary>
    /// Indicates a warning message.
    /// </summary>
    Warning,

    /// <summary>
    /// Indicates an informational message.
    /// </summary>
    Info,

    /// <summary>
    /// Indicates a hint or suggestion message.
    /// </summary>
    Hint,
}
