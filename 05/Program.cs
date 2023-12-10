using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new(@"\d+");
    // TODO IMPROVE! Ideally a baackwards (or forwards) approach mapping intervals should work

    private static void Main(string[] args)
    {
        const int day = 5;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        var firstLine = streamReader.ReadLine()!;
        var seeds = GetSeeds(firstLine);

        var maps = ParseInput(streamReader).ToArray();

        var currentTestedValue = 0L;
        const long maxIterations = 150000000;
        while (currentTestedValue < maxIterations)
        {
            if (currentTestedValue % 10000 == 0)
            {
                System.Console.WriteLine($"iteration {currentTestedValue}");
            }
            var counterImages = new HashSet<long> { currentTestedValue };

            for (int i = maps.Length - 1; i >= 0; i--)
            {
                counterImages = maps[i].CounterImage(counterImages);
            }

            // check if any of the counterimages is in the seed collection
            if (counterImages.Any(ci => seeds.Any(s => s.ContainsValue(ci))))
            {
                System.Console.WriteLine($"Valid seed found! it goes into location {currentTestedValue}");
                return;
            }

            currentTestedValue++;
        }
        System.Console.WriteLine("Too bad, no eligible seed was found");
    }


    private static IEnumerable<Seeds> GetSeeds(string firstLine)
    {
        var numbers = GetSeedsSimple(firstLine).ToArray();
        for (int i = 0; i < numbers.Length; i += 2)
        {
            yield return new Seeds
            {
                RangeStart = numbers[i],
                RangeLength = numbers[i + 1]
            };
        }
    }
    private static IEnumerable<long> GetSeedsSimple(string firstLine) =>
        NumberRegex.Matches(firstLine).Select(m => long.Parse(m.Value));

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

    public HashSet<long> CounterImage(HashSet<long> values) =>
        values.SelectMany(l => GetCounterImage(l))
              .ToHashSet();

    private HashSet<long> GetCounterImage(long value)
    {
        var counterImages = MapItems
            .Where(mi => mi.IsValueInOutputRange(value))
            .Select(mi => mi.CounterImage(value))
            .ToHashSet();

        // add the value itself if there is no map mapping it directly
        if (MapItems.All(mi => !mi.ShouldApply(value)))
        {
            counterImages.Add(value);
        }
        return counterImages;
    }
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

    public bool IsValueInOutputRange(long value) =>
        DestinationRangeStart <= value && value < (DestinationRangeStart + RangeLength);

    public long CounterImage(long value)
    {
        if (!IsValueInOutputRange(value))
        {
            throw new Exception("Ma es mona?");
        }

        var offset = value - DestinationRangeStart;
        return SourceRangeStart + offset;
    }
}

internal sealed class Seeds
{
    public required long RangeStart { get; init; }
    public required long RangeLength { get; init; }

    public long GetMinimumApplicationResult(Func<long, long> map)
    {
        var result = long.MaxValue;

        for (int i = 0; i < RangeLength; i++)
        {
            var currentReulst = map(RangeStart + i);
            result = currentReulst < result
                ? currentReulst
                : result;
        }

        return result;
    }

    internal bool ContainsValue(long value) =>
        RangeStart <= value && value < RangeStart + RangeLength;
}