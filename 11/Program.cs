internal class Program
{
    public static void Main(string[] args)
    {
        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);
        var universe = ParseInput(streamReader).ToArray();
        var rowsToExpand = universe
            .Select((r, i) => (Row: r, Index: i))
            .Where(tuple => tuple.Row.All(c => c == '.'))
            .Select(tuple => tuple.Index);

        var columnsToExpand = universe[0]
            .Select((_, j) => j)
            .Where(j => universe
                .Select(row => row[j])
                .All(c => c == '.')
            );

        var galaxiesPositions = FindGalaxies(universe).ToArray();

        // saving distances in case they are needed for part 2
        var distances = new int[galaxiesPositions.Length, galaxiesPositions.Length];

        var sum = 0;

        for (int i = 0; i < galaxiesPositions.Length; i++)
        {
            var currentGalaxy = galaxiesPositions[i];
            for (int j = i + 1; j < galaxiesPositions.Length; j++)
            {
                var otherGalaxy = galaxiesPositions[j];
                var distance = Math.Abs(currentGalaxy.RowIdx - otherGalaxy.RowIdx)
                               + Math.Abs(currentGalaxy.ColIdx - otherGalaxy.ColIdx);
                var realdistance = distance + CountExpansionBetween(currentGalaxy, otherGalaxy, rowsToExpand, columnsToExpand);
                sum += realdistance;
                distances[i, j] = realdistance;
            }
        }

        Console.WriteLine(sum);
    }

    private static int CountExpansionBetween((int RowIdx, int ColIdx) currentGalaxy, (int RowIdx, int ColIdx) otherGalaxy, IEnumerable<int> rowsToExpand, IEnumerable<int> columnsToExpand)
    {
        var rowExpansions = rowsToExpand.Count(i => Math.Min(currentGalaxy.RowIdx, otherGalaxy.RowIdx) < i && i < Math.Max(currentGalaxy.RowIdx, otherGalaxy.RowIdx));
        var colExpansions = columnsToExpand.Count(j => Math.Min(currentGalaxy.ColIdx, otherGalaxy.ColIdx) < j && j < Math.Max(currentGalaxy.ColIdx, otherGalaxy.ColIdx));
        return rowExpansions + colExpansions;
    }
    

    private static IEnumerable<(int RowIdx, int ColIdx)> FindGalaxies(char[][] universe)
    {
        for (int i = 0; i < universe.Length; i++)
        {
            for (int j = 0; j < universe[0].Length; j++)
            {
                if (universe[i][j] == '#')
                {
                    yield return (i, j);
                }
            }
        }
    }

    private static IEnumerable<char[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!.ToCharArray();
        }
    }
}