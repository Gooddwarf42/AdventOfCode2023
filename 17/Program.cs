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

        var minimumPaths = GetMinimumPaths(map);

        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                System.Console.Write(minimumPaths[i, j] + " ");
            }
            System.Console.WriteLine();
        }

        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");
        System.Console.WriteLine(minimumPaths[map.Length - 1, map[0].Length - 1]);
    }

    private static int[,] GetMinimumPaths(int[][] map)
    {
        var minimumPaths = new int[map.Length, map[0].Length];
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                var neighbours = new[] { (Row: i + 1, Col: j), (Row: i, Col: j + 1), (Row: i - 1, Col: j), (Row: i, Col: j - 1) };
                var validNeighbours = neighbours.Where(n => !IsOutOfBounds(n)); // Todo: invent somethign to check here validity of that movement

                foreach (var (row, col) in validNeighbours)
                {
                    var currentMinPath = minimumPaths[row, col];
                    var newPossibleMinPath = minimumPaths[i, j] + map[row][col];
                    if (currentMinPath == 0 || newPossibleMinPath < currentMinPath)
                    {
                        minimumPaths[row, col] = newPossibleMinPath;
                    }
                }
            }
        }


        return minimumPaths;

        bool IsOutOfBounds((int i, int j) coordinate)
            => (coordinate.i < 0)
               || (coordinate.j < 0)
               || (coordinate.i >= map.Length)
               || (coordinate.j >= map[0].Length);
    }

    private static IEnumerable<int[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!
                .ToCharArray() // I am too lazy to check if there is a method splitting a string in parts of fixed length
                .Select(c => int.Parse(c.ToString()))
                .ToArray();
        }
    }
}