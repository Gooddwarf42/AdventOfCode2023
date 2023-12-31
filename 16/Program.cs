﻿// See https://aka.ms/new-console-template for more information

using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

internal class Program
{
    public static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();

        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);

        var map = ParseInput(streamReader).ToArray();

        var startingPoints = getStartingPoints(map);

        var maxEnergizedTiles = startingPoints
            .Select(sp => EnergizeFrom(map, sp))
            .Max();


        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");

        System.Console.WriteLine(maxEnergizedTiles);
    }

    private static IEnumerable<LazerIsHereTile> getStartingPoints(char[][] map)
    {
        var height = map.Length;
        var width = map[0].Length;

        // get horizontal starting points
        for (int i = 0; i < height; i++)
        {
            yield return new LazerIsHereTile
            {
                Coordinates = (i, 0),
                Entering = Direction.L,
                TileType = map[i][0],
            };
            yield return new LazerIsHereTile
            {
                Coordinates = (i, width - 1),
                Entering = Direction.R,
                TileType = map[i][width - 1],
            };
        }

        // get horizontal starting points
        for (int j = 0; j < width; j++)
        {
            yield return new LazerIsHereTile
            {
                Coordinates = (0, j),
                Entering = Direction.U,
                TileType = map[0][j],
            };
            yield return new LazerIsHereTile
            {
                Coordinates = (height - 1, j),
                Entering = Direction.D,
                TileType = map[height - 1][j],
            };
        }
    }

    private static int EnergizeFrom(char[][] map, LazerIsHereTile startingPoint)
    {
        // I shall be lazy this time
        var energizedMapTracker = new Direction[map.Length][];
        for (int i = 0; i < map[0].Length; i++)
        {
            energizedMapTracker[i] = new Direction[map[0].Length];
        }

        EnergizeMap(startingPoint, map, energizedMapTracker);

        var energizedTiles =
            energizedMapTracker
                .Select(row => row.Count(tile => tile != 0))
                .Sum();

        return energizedTiles;
    }

    private static void EnergizeMap(LazerIsHereTile currentTile, char[][] map, Direction[][] energizedMap)
    {
        var coordinates = currentTile.Coordinates;

        if (energizedMap[coordinates.i][coordinates.j].HasFlag(currentTile.Entering))
        {
            return;
        }

        energizedMap[coordinates.i][coordinates.j] |= currentTile.Entering;

        var nextDirections = currentTile.GetLeavingDirections();

        foreach (var direction in nextDirections)
        {
            var newCoordinates = direction.PerformOn(coordinates);

            if (IsOutOfBounds(newCoordinates))
            {
                continue;
            }

            var newTile = new LazerIsHereTile()
            {
                Coordinates = newCoordinates,
                Entering = direction.RotateNibbleLeft(2),
                TileType = map[newCoordinates.i][newCoordinates.j],
            };

            EnergizeMap(newTile, map, energizedMap);
        }

        return;

        bool IsOutOfBounds((int i, int j) coordinate)
            => (coordinate.i < 0)
               || (coordinate.j < 0)
               || (coordinate.i >= map.Length)
               || (coordinate.j >= map[0].Length);
    }

    private static IEnumerable<char[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!.ToCharArray();
        }
    }
}

// Madonna che agricolo...
internal sealed class LazerIsHereTile
{
    public required char TileType { get; set; }
    public required (int i, int j) Coordinates { get; init; }
    public required Direction Entering { get; set; }

    public IEnumerable<Direction> GetLeavingDirections() =>
        TileType switch
        {
            '.' => [Entering.RotateNibbleLeft(2)],
            '/' => HandleSlash(Entering),
            '\\' => HandleBackslash(Entering),
            '|' => HandlePipe(Entering),
            '-' => HandleDash(Entering),
            _ => throw new ArgumentOutOfRangeException()
        };

    private static IEnumerable<Direction> HandleSlash(Direction entering) =>
        entering switch
        {
            Direction.U => [Direction.L],
            Direction.R => [Direction.D],
            Direction.D => [Direction.R],
            Direction.L => [Direction.U],
            _ => throw new ArgumentOutOfRangeException(nameof(entering), entering, null)
        };

    private static IEnumerable<Direction> HandleBackslash(Direction entering) =>
        entering switch
        {
            Direction.U => [Direction.R],
            Direction.R => [Direction.U],
            Direction.D => [Direction.L],
            Direction.L => [Direction.D],
            _ => throw new ArgumentOutOfRangeException(nameof(entering), entering, null)
        };

    private static IEnumerable<Direction> HandlePipe(Direction entering) =>
        entering switch
        {
            Direction.R or Direction.L => [Direction.U, Direction.D],
            Direction.U => [Direction.D],
            Direction.D => [Direction.U],
            _ => throw new ArgumentOutOfRangeException(nameof(entering), entering, null)
        };

    private static IEnumerable<Direction> HandleDash(Direction entering) =>
        entering switch
        {
            Direction.U or Direction.D => [Direction.L, Direction.R],
            Direction.L => [Direction.R],
            Direction.R => [Direction.L],
            _ => throw new ArgumentOutOfRangeException(nameof(entering), entering, null)
        };
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