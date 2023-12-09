
using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new(@"\d+");

    private static void Main(string[] args)
    {
        const int day = 5;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        // TODO get seeds
        var firstLine = streamReader.ReadLine()!;
        var seeds = NumberRegex.Matches(firstLine).Select(m => long.Parse(m.Value));

        var maps = ParseInput(streamReader);

        var result = seeds;

        foreach(var map in maps)
        {
            result = result.Select(map.Apply);
        }

        System.Console.WriteLine(result.Min());
    }

    private static IEnumerable<Map> ParseInput(StreamReader streamReader)
    {
        Map? mapBeingParsed = null;

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.Contains("map"))
            {
                if (mapBeingParsed is not null)
                {
                    yield return mapBeingParsed;
                }

                mapBeingParsed = new();
                continue;
            }

            MapItem mapItem = ParseMapItem(line);
            mapBeingParsed!.MapItems.Add(mapItem);
        }

        // Bleh, I still have to return the last map, unless I opt for sketchy shenanigans on input
        yield return mapBeingParsed!;
    }

    private static MapItem ParseMapItem(string line)
    {
        var mapItemRaw = NumberRegex.Matches(line).Select(m => long.Parse(m.Value)).ToArray();

        if (mapItemRaw.Length != 3)
        {
            throw new Exception("Something bad!");
        }

        return new MapItem
        {
            DestinationRangeStart = mapItemRaw[0],
            SourceRangeStart = mapItemRaw[1],
            RangeLength = mapItemRaw[2],
        };
    }
}

internal sealed class Map
{
    public List<MapItem> MapItems { get; set; } = new();

    public long Apply(long input) =>
        MapItems
            .SingleOrDefault(mi => mi.ShouldApply(input)) // I guess I could replace this with FirstOrDefault, but I am curious to see if I get exceptions this way
            ?.Apply(input)
            ?? input;
}

internal sealed class MapItem
{
    public required long DestinationRangeStart { get; init; }
    public required long SourceRangeStart { get; init; }
    public required long RangeLength { get; init; }

    public bool ShouldApply(long input) =>
        SourceRangeStart <= input && input < (SourceRangeStart + RangeLength);

    public long Apply(long input)
    {
        if (!ShouldApply(input))
        {
            throw new Exception("Ma es mona?");
        }

        var offset = input - SourceRangeStart;
        return DestinationRangeStart + offset;
    }
}