using System.Collections.Generic;

namespace Silverfly;

public abstract class PrecedenceLevels
{
    private readonly List<string> precedenceLevels = new List<string>();

    public int GetPrecedence(string level)
    {
        var index = precedenceLevels.IndexOf(level);

        return index == -1 ? precedenceLevels.Count : index;
    }

    public void AddPrecedence(string level)
    {
        if (!precedenceLevels.Contains(level))
        {
            precedenceLevels.Add(level);
        }
    }

    public void InsertPrecedence(int index, string level)
    {
        if (!precedenceLevels.Contains(level))
        {
            precedenceLevels.Insert(index, level);
        }
    }
}