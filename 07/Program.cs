using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new(@"\d+");

    private static void Main(string[] args)
    {
        const int day = 7;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);
    }
}