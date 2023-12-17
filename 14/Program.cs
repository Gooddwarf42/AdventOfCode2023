using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();

        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);
        var map = ParseInput(streamReader).ToArray();

        var computedWeight = ComputeWeight(map);


        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");
        System.Console.WriteLine(computedWeight);


    }

    private static int ComputeWeight(char[][] map)
    {
        var totalWeight = 0;
        var mapHeight = map.Length;
        var mapWidth = map[0].Length;
        var highestBoulderRollingPoint = new int[mapWidth];

        // scan the map and do the appropriate things
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                switch (map[i][j])
                {
                    case '.':
                        continue;
                    case 'O':
                        totalWeight += mapHeight - highestBoulderRollingPoint[j];
                        highestBoulderRollingPoint[j]++;
                        break;
                    case '#':
                        highestBoulderRollingPoint[j] = i + 1;
                        break;
                    default:
                        throw new Exception("unexpected character");
                }
            }
        }

        return totalWeight;
    }

    private static IEnumerable<char[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            yield return line!.ToCharArray();
        }
    }
}