using System;
using System.Collections.Generic;

namespace Silverfly.Helpers;

public static class LevensteinDistance
{
    public static int Calculate(string source, string target)
    {
        if (string.IsNullOrEmpty(source))
        {
            return string.IsNullOrEmpty(target) ? 0 : target.Length;
        }

        if (string.IsNullOrEmpty(target))
        {
            return source.Length;
        }

        if (source.Length > target.Length)
        {
            (source, target) = (target, source);
        }

        var m = target.Length;
        var n = source.Length;
        var distance = new int[2, m + 1];
        // Initialize the distance matrix
        for (var j = 1; j <= m; j++)
        {
            distance[0, j] = j;
        }

        var currentRow = 0;
        for (var i = 1; i <= n; ++i)
        {
            currentRow = i & 1;
            distance[currentRow, 0] = i;
            var previousRow = currentRow ^ 1;
            for (var j = 1; j <= m; j++)
            {
                var cost = target[j - 1] == source[i - 1] ? 0 : 1;
                distance[currentRow, j] = Math.Min(Math.Min(
                        distance[previousRow, j] + 1,
                        distance[currentRow, j - 1] + 1),
                    distance[previousRow, j - 1] + cost);
            }
        }
        return distance[currentRow, m];
    }

    public static string Suggest(string source, IEnumerable<string> possibilities)
    {
        var lastDistance = 10;
        var lastSuggestion = string.Empty;

        foreach (var str in possibilities)
        {
            var distance = Calculate(source, str);

            if (distance <= lastDistance)
            {
                lastDistance = distance;
                lastSuggestion = str;
            }
        }

        return lastSuggestion;
    }
}
