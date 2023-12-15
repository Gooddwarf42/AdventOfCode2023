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
            .Select((c, i) => (Col: c, Index: i))
            .Where(tuple => universe
                .Select(r => r[tuple.Index])
                .All(c => c == '.')
            )
            .Select(tuple => tuple.Index);
        
        
    }

    private static IEnumerable<char[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!.ToCharArray();
        }
    }
}