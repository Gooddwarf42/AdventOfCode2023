// See https://aka.ms/new-console-template for more information

internal class Program
{
    private static readonly Dictionary<char, Direction> PipeCharArray = new()
    {
        { '|', Direction.U | Direction.D },
        { '-', Direction.L | Direction.R },
        { 'L', Direction.U | Direction.R },
        { 'J', Direction.L | Direction.U },
        { '7', Direction.L | Direction.D },
        { 'F', Direction.R | Direction.D },
        { 'S', Direction.All }
    };

    public static void Main(string[] args)
    {
        var inputPath = $"input.txt";
        using var streamReader = new StreamReader(inputPath);
        var map = ParseInput(streamReader).ToList();
        List<LoopTile> loop = FindLoop(map);
        var area = GetLoopArea(loop);
        Console.WriteLine(area);
    }

    private static int GetLoopArea(List<LoopTile> loop)
    {
        var scannedLoop = loop
            .OrderBy(lt => lt.Coordinates.j)
            .GroupBy(lt => lt.Coordinates.i)
            .ToArray();

        var area = 0;
        foreach (var row in scannedLoop)
        {
            var verticalWalls = GerVerticalWalls(row).ToArray();

            for (int k = 0; k < verticalWalls.Length; k += 2)
            {
                // | . . L - - J . . |
                // | . . . . . |
                var totalSpacesInBetween = verticalWalls[k + 1].Tile.Coordinates.j
                    - verticalWalls[k].Tile.Coordinates.j
                    - 1;
                var otherWallsInBetween = verticalWalls[k + 1].Index
                    - verticalWalls[k].Index
                    - 1;

                area += totalSpacesInBetween - otherWallsInBetween;
            }
        }

        return area;
    }

    private static IEnumerable<(LoopTile Tile, int Index)> GerVerticalWalls(IGrouping<int, LoopTile> row)
    {
        var rowWithIndices = row
            .Select((t, i) => (Tile: t, Index: i));

        var allVerticalWalls = new[] { '|', '7', 'F' };

        return rowWithIndices.Where(t => allVerticalWalls.Contains(t.Tile.TileType));
        // foreach (var loopTile in rowWithInices)
        // {

        //     // if (lookingForOpening && openingWalls.Contains(loopTile.Tile.TileType)
        //     //     || !lookingForOpening && closingWalls.Contains(loopTile.Tile.TileType))
        //     // {
        //     //     lookingForOpening = !lookingForOpening;
        //     //     yield return loopTile;
        //     // }
        // }
    }

    private static List<LoopTile> FindLoop(List<char[]> map)
    {
        var loop = new List<LoopTile>();

        var currentPosition = FindS(map);
        var currentPositionChar = 'S';
        var lastMovement = Direction.U; //Irrelevant value

        do
        {
            var cameFrom = lastMovement.RotateNibbleLeft(2);
            var nextMovement = currentPositionChar == 'S'
                ? FindMovementToAdjacentUseablePipe(map, currentPosition)
                : FindNextMovement(currentPositionChar, cameFrom);

            loop.Add(new()
            {
                TileType = currentPositionChar,
                Coordinates = currentPosition,
                Entering = cameFrom, //note: for the S tile, this is wrong! Adjust it later!
                Leaving = nextMovement,
            });

            var newPosition = nextMovement.PerformOn(currentPosition);

            lastMovement = nextMovement;
            currentPosition = newPosition;
            currentPositionChar = map[currentPosition.i][currentPosition.j];
        } while (currentPositionChar != 'S');

        // fix S Entering value and TileType
        loop[0].Entering = loop.Last().Leaving.RotateNibbleLeft(2);
        loop[0].TileType = PipeCharArray
            .Single(kvp => kvp.Value == (loop[0].Entering | loop[0].Leaving))
            .Key;

        return loop;
    }

    private static Direction FindNextMovement(char currentPositionChar, Direction cameFrom)
    {
        var availableDirections = PipeCharArray[currentPositionChar];
        return availableDirections ^ cameFrom;
    }

    private static Direction FindMovementToAdjacentUseablePipe(List<char[]> map, (int i, int j) currentPosition)
    {
        var currentAttempt = Direction.U;
        for (int i = 0; i < 4; i++)
        {
            currentAttempt = currentAttempt.RotateNibbleLeft(1);

            var newPosition = currentAttempt.PerformOn(currentPosition);
            if (IsMovementToHereValid(map[newPosition.i][newPosition.j], currentAttempt.RotateNibbleLeft(2)))
            {
                return currentAttempt;
            }
        }

        throw new Exception("MIMMO");
    }

    private static bool IsMovementToHereValid(char c, Direction lastMovement)
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

internal sealed class LoopTile
{
    public required char TileType { get; set; }
    public required (int i, int j) Coordinates { get; init; }
    public required Direction Entering { get; set; }
    public required Direction Leaving { get; init; }
}

[Flags]
internal enum Direction
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
    public static Direction ChangeMovement(this Direction source, int offset) =>
        (Direction)((int)(source + offset) % 4);


    public static Direction RotateNibbleLeft(this Direction source, int offset) =>
        offset > 3
            ? throw new ArgumentException(null, nameof(offset))
            : (Direction)((((uint)source << offset) | ((uint)source >> 4 - offset)) & (uint)0b1111);


    public static (int i, int j) PerformOn(this Direction source, (int i, int j) position) =>
        source switch
        {
            Direction.U => (position.i - 1, position.j),
            Direction.R => (position.i, position.j + 1),
            Direction.D => (position.i + 1, position.j),
            Direction.L => (position.i, position.j - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
}