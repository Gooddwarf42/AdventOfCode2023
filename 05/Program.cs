
internal class Program
{
    private static void Main(string[] args)
    {
        const int day = 5;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        // TODO get seeds

        var maps = ParseInput(streamReader);
    }

    private static IEnumerable<Map> ParseInput(StreamReader streamReader)
    {
        throw new NotImplementedException();
    }
}

internal sealed class Map
{
    public List<MapItem> mapItems { get; set; } = new();

    public int Apply(int input)
    {
        return mapItems
            .Where(mi => mi.ShouldApply(input))
            .SingleOrDefault() // I guess I could replace these with FirstOrDefault, but I am curious to see if I get exceptions this way
            ?.Apply(input)
            ?? input;
    }
}

internal sealed class MapItem
{
    public required int DestinationRangeStart { get; init; }
    public required int SourceRangeStart { get; init; }
    public required int RangeLength { get; init; }

    public bool ShouldApply(int input) =>
        SourceRangeStart <= input && input < (SourceRangeStart + RangeLength);

    public int Apply(int input)
    {
        if (!ShouldApply(input))
        {
            throw new Exception("Ma es mona?");
        }

        var offset = input - SourceRangeStart;
        return DestinationRangeStart + offset;
    }
}