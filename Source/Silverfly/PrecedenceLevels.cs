using System.Collections.Generic;

namespace Silverfly;

public abstract class PrecedenceLevels
{
    private readonly List<string> precedenceLevels = new();

    /// <summary>
    ///     Gets the precedence level of the specified level string.
    /// </summary>
    /// <param name="level">The precedence level string whose index is to be retrieved.</param>
    /// <returns>
    ///     The index of the specified precedence level in the list, or the count of precedence levels if the level is not
    ///     found.
    /// </returns>
    /// <remarks>
    ///     This method returns the position of the specified <paramref name="level" /> string in the list of precedence
    ///     levels.
    ///     If the level is not found, it returns the total count of precedence levels, indicating a lower precedence.
    /// </remarks>
    public int GetPrecedence(string level)
    {
        var index = precedenceLevels.IndexOf(level);

        return index == -1 ? precedenceLevels.Count : index;
    }

    /// <summary>
    ///     Adds a new precedence level to the list if it does not already exist.
    /// </summary>
    /// <param name="level">The precedence level string to add.</param>
    /// <remarks>
    ///     This method adds the specified <paramref name="level" /> string to the list of precedence levels if it is not
    ///     already present.
    ///     This ensures that the list of precedence levels remains unique and up-to-date.
    /// </remarks>
    public void AddPrecedence(string level)
    {
        if (!precedenceLevels.Contains(level))
        {
            precedenceLevels.Add(level);
        }
    }

    /// <summary>
    ///     Inserts a new precedence level at the specified index if it does not already exist.
    /// </summary>
    /// <param name="index">The index at which to insert the precedence level.</param>
    /// <param name="level">The precedence level string to insert.</param>
    /// <remarks>
    ///     This method inserts the specified <paramref name="level" /> string into the list of precedence levels at the given
    ///     <paramref name="index" />.
    ///     If the level is not already present in the list, it is added at the specified position.
    /// </remarks>
    public void InsertPrecedence(int index, string level)
    {
        if (!precedenceLevels.Contains(level))
        {
            precedenceLevels.Insert(index, level);
        }
    }
}
