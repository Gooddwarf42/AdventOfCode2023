// See https://aka.ms/new-console-template for more information

internal class Program
{
    private static readonly Dictionary<char, Directions> PipeCharArray = new()
    {
        { '|', Directions.U | Directions.D },
        { '-', Directions.L | Directions.R },
        { 'L', Directions.U | Directions.R },
        { 'J', Directions.L | Directions.U },
        { '7', Directions.L | Directions.D },
        { 'F', Directions.R | Directions.D },
        { 'S', Directions.All }
    };

    public static void Main(string[] args)
    {
        var inputPath = $"input.txt";
        using var streamReader = new StreamReader(inputPath);
        var map = ParseInput(streamReader).ToList();
        foreach (var line in map)
        {
            Console.WriteLine(string.Join(null, line));
        }

        var currentPosition = FindS(map);
        var nextPositionChar = 'S'; //random unused character to denote not initialized variable
        var countSteps = 0;
        var lastMovement = Directions.U;
        var newPosition = (i: -1, j: -1);

        do
        {
            var cameFrom = lastMovement.RotateNibbleLeft(2);

            //find next pipe
            for (int i = 1; i <= 3; i++)
            {
                lastMovement = cameFrom.RotateNibbleLeft(i);
                newPosition = lastMovement.PerformOn(currentPosition);

                if (!PipeCharArray[nextPositionChar].HasFlag(lastMovement))
                {
                    continue;
                }

                if (IsMovementValid(map[newPosition.i][newPosition.j], lastMovement.RotateNibbleLeft(2)))
                {
                    Console.WriteLine($"Valid movement: {lastMovement.ToString()}");
                    Console.WriteLine($"Moving to: {newPosition}");
                    break;
                }

                if (i == 3)
                {
                    throw new Exception("Sanity check qui non arrivo");
                }
            }

            currentPosition = newPosition;
            nextPositionChar = map[currentPosition.i][currentPosition.j];
            Console.WriteLine($"New char: {nextPositionChar}");
            Console.WriteLine("");

            countSteps++;
        } while (nextPositionChar != 'S');

        Console.WriteLine(countSteps / 2);
    }

    private static bool IsMovementValid(char c, Directions lastMovement)
    {
        var cIsPipeChar = PipeCharArray.TryGetValue(c, out var validDirections);
        return cIsPipeChar && validDirections.HasFlag(lastMovement);
    }

    private static (int i, int j) FindS(List<char[]> map)
    {
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (map[i][j] == 'S')
                {
                    return (i, j);
                }
            }
        }

        throw new Exception("no S found");
    }

    private static IEnumerable<char[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!.ToCharArray();
        }
    }
}

[Flags]
internal enum Directions
{
    U = 1,
    R = 2,
    D = 4,
    L = 8,
    All = U | R | D | L,
}

internal static class DirectionsExtensions
{
    // assume positive offset
    public static Directions ChangeMovement(this Directions source, int offset) =>
        (Directions)((int)(source + offset) % 4);


    public static Directions RotateNibbleLeft(this Directions source, int offset) =>
        offset > 3
            ? throw new ArgumentException(null, nameof(offset))
            : (Directions)((((uint)source << offset) | ((uint)source >> 4 - offset)) & (uint)0b1111);


    public static (int i, int j) PerformOn(this Directions source, (int i, int j) position) =>
        source switch
        {
            Directions.U => (position.i - 1, position.j),
            Directions.R => (position.i, position.j + 1),
            Directions.D => (position.i + 1, position.j),
            Directions.L => (position.i, position.j - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}