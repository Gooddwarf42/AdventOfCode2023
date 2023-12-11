// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex LocationRegex = new(@"([A-Z]|\d){3}");

    private static void Main(string[] args)
    {
        const int day = 8;
        var inputPath = $"input.txt";
        using var streamReader = new StreamReader(inputPath);
        var movementSequence = streamReader.ReadLine()!.Trim().ToArray();
        _ = streamReader.ReadLine(); //discard empty line

        var maps = ParseInputMaps(streamReader);

        var currentLocations = maps.Keys.Where(s => s.EndsWith('A')).ToArray();
        var firstZs = currentLocations.Select(s => StepsToFirstZ(movementSequence, maps, s));

        var lcm = 1L;
        foreach(var value in firstZs){
            lcm = LCM(lcm, value);
        }

        Console.WriteLine(lcm);
    }

    private static long GCD(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static long LCM(long a, long b)
    {
        return (a / GCD(a, b)) * b;
    }

    private static long StepsToFirstZ(char[] movementSequence, Dictionary<string, MapItem> maps, string currentLocation)
    {
        var countSteps = 0L;
        var currentMoveIndex = 0;
        while (!currentLocation.EndsWith('Z'))
        {
            var mapItem = maps[currentLocation];
            currentLocation = movementSequence[currentMoveIndex] switch
            {
                'L' => mapItem.Left,
                'R' => mapItem.Right,
                _ => throw new Exception("mannaggia al mimmo")
            };

            currentMoveIndex = (currentMoveIndex + 1) % movementSequence.Length;
            countSteps++;
        }

        return countSteps;
    }

    private static Dictionary<string, MapItem> ParseInputMaps(StreamReader streamReader)
    {
        var dict = new Dictionary<string, MapItem>();

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            var locations = LocationRegex.Matches(line!).Select(m => m.Value).ToArray();
            dict.Add(locations[0], new MapItem()
            {
                Left = locations[1],
                Right = locations[2],
            });
        }

        return dict;
    }
}

internal sealed class MapItem
{
    public required string Left { get; init; }
    public required string Right { get; init; }
}