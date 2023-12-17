


using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();

        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);
        var patterns = ParseInput(streamReader);

        var sumOfSummaries = 0;
        foreach (var pattern in patterns)
        {
            sumOfSummaries += GetSummary(pattern);
        }

        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");

        System.Console.WriteLine(sumOfSummaries);
    }

    private static int CharArrayDistance(char[] a, char[] b)
    {
        if (a.Length != b.Length)
        {
            throw new Exception("Invalid use of this method");
        }
        var count = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                count++;
            }
        }
        return count;
    }

    private static int GetSummary(char[][] pattern)
    {
        var horizontalReflexion = FindHorizontalReflection(pattern);
        if (horizontalReflexion is not null)
        {
            return 100 * (horizontalReflexion.Value + 1);
        }

        var verticalReflection = FindVerticalReflection(pattern);
        if (verticalReflection is not null)
        {
            return verticalReflection.Value + 1;
        }

        throw new Exception("No reflection found");
    }

    private static int? FindVerticalReflection(char[][] pattern)
    {
        var stringifiedCols = StringifyByCol(pattern).ToArray();
        return FindReflectionInStringified(stringifiedCols);
    }

    private static int? FindHorizontalReflection(char[][] pattern)
    {
        var stringifiedRows = StringifyByRow(pattern).ToArray();
        return FindReflectionInStringified(stringifiedRows);
    }

    private static int? FindReflectionInStringified(char[][] stringifiedLines)
    {
        for (int i = 0; i < stringifiedLines.Length - 1; i++)
        {
            if (CheckSimmetryInStringified(stringifiedLines, i))
            {
                return i;
            }
        }
        return null;
    }

    private static bool CheckSimmetryInStringified(char[][] stringifiedLines, int reflectionIndex)
    {
        var i = reflectionIndex;
        var j = reflectionIndex + 1;
        var smudgeFound = 0;

        while (i >= 0 && j < stringifiedLines.Length)
        {
            var distance = CharArrayDistance(stringifiedLines[i], stringifiedLines[j]);
            if (distance > 1)
            {
                // no way!
                return false;
            }
            if (distance == 1)
            {
                //potential smudge
                smudgeFound++;
            }

            if (smudgeFound > 1)
            {
                // too many smudges!
                return false;
            }

            i--;
            j++;
        }
        return smudgeFound == 1;
    }

    //pretty much useless, but keeps things symmetric xD
    private static IEnumerable<char[]> StringifyByRow(char[][] pattern) =>
        pattern.AsEnumerable();
    private static IEnumerable<char[]> StringifyByCol(char[][] pattern) =>
        pattern[0]
            .Select((_, j) => pattern.Select(row => row[j]).ToArray());

    private static IEnumerable<char[][]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return ParsePattern(streamReader).ToArray();
        }
    }
    private static IEnumerable<char[]> ParsePattern(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            yield return line.ToCharArray();
        }
    }
}

