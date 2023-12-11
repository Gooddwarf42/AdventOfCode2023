// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex LocationRegex = new(@"([A-Z]|\d){3}");

    private static void Main(string[] args)
    {
        const int day = 8;
        var inputPath = $"input2.txt";
        using var streamReader = new StreamReader(inputPath);
        var movementSequence = streamReader.ReadLine()!.Trim().ToArray();
        _ = streamReader.ReadLine(); //discard empty line

        var maps = ParseInputMaps(streamReader);

        var currentLocations = maps.Keys.Where(s => s.EndsWith('A')).ToArray();

        var countSteps = 0;
        var currentMoveIndex = 0;
        while (!currentLocations.All(s => s.EndsWith('Z')))
        {
            for (int i = 0; i < currentLocations.Length; i++)
            {
                var mapItem = maps[currentLocations[i]];
                currentLocations[i] = movementSequence[currentMoveIndex] switch
                {
                    'L' => mapItem.Left,
                    'R' => mapItem.Right,
                    _ => throw new Exception("mannaggia al mimmo")
                };
            }
            
            currentMoveIndex = (currentMoveIndex + 1) % movementSequence.Length;
            countSteps++;
        }

        Console.WriteLine(countSteps);
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