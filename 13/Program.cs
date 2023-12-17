


internal class Program
{
    private static void Main(string[] args)
    {
        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);
        var patterns = ParseInput(streamReader);

        var sumOfSummaries = 0;
        foreach (var pattern in patterns)
        {
            sumOfSummaries += GetSummary(pattern);
        }
        System.Console.WriteLine(sumOfSummaries);
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

    private static int? FindReflectionInStringified(string[] stringifiedLines)
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

    private static bool CheckSimmetryInStringified(string[] stringifiedLines, int reflectionIndex)
    {
        var i = reflectionIndex;
        var j = reflectionIndex + 1;
        var isSymmetric = true;

        while (i >= 0 && j < stringifiedLines.Length)
        {
            if (stringifiedLines[i] != stringifiedLines[j])
            {
                isSymmetric = false;
                break;
            }
            i--;
            j++;
        }
        return isSymmetric;

    }

    private static IEnumerable<string> StringifyByRow(char[][] pattern) =>
        pattern.Select(charArray => string.Join(null, charArray));
    private static IEnumerable<string> StringifyByCol(char[][] pattern)
    {
        var columnsEnumerable = pattern[0]
            .Select((_, j) => pattern.Select(row => row[j]).ToArray()); //huh, this pretty much transposes a matrix, cool
        return columnsEnumerable.Select(charArray => string.Join(null, charArray));
    }

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

