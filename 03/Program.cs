using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex _notDigitRegex = new Regex(@"\D");
    private static readonly Regex _digitOrDotRegex = new Regex(@"\d|\.");
    private static readonly Regex _numberRegex = new Regex(@"\d+");
    private static void Main(string[] args)
    {
        const int day = 3;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        var inputPadded = GetPaddedInput(streamReader);

        // foreach (var line in inputPadded)
        // {
        //     Console.WriteLine(string.Join(null, line));
        // }

        
        var total = SolvePart1(inputPadded);

        System.Console.WriteLine(total);
    }

    private static int SolvePart1(char[][] inputPadded)
    {
        var total = 0;
        
        var listOfGoodNumbers = new List<int>();
        var currentNumberStringBuilder = new StringBuilder();

        var parsingNumState = false;
        var isGoodNumber = false;

        for (int i = 1; i < inputPadded.Length - 1; i++)
        {
            var row = inputPadded[i];
            for (int j = 1; j < row.Length - 1; j++)
            {
                var currentChar = row[j];
                if (_notDigitRegex.Match(currentChar.ToString()).Success)
                {
                    if (!parsingNumState || !isGoodNumber)
                    {
                        parsingNumState = false;
                        continue;
                    }

                    // finished parsing a good number, add it to the good list of numbers
                    var toAdd = int.Parse(currentNumberStringBuilder.ToString());
                    total += int.Parse(currentNumberStringBuilder.ToString());

                    parsingNumState = false;
                    isGoodNumber = false;

                    continue;
                }

                // current character is a digit. 

                // Initialize parsing if needed
                if (!parsingNumState)
                {
                    currentNumberStringBuilder = new();
                    parsingNumState = true;
                }

                currentNumberStringBuilder.Append(currentChar);

                // Current parse already confirmed number to be a good boy
                if (isGoodNumber)
                {
                    continue;
                }

                // Check if current digit is good and act accordingly
                if (IsDigitGood(i, j, inputPadded))
                {
                    isGoodNumber = true;
                }

            }
        }

        return total;
    }

    private static bool IsDigitGood(int i, int j, char[][] inputPadded)
    {
        var neighbours = GetNeighbours(i, j, inputPadded);

        // System.Console.WriteLine("Neighbours found:");
        // System.Console.WriteLine(string.Join(null, neighbours));

        return neighbours.Any(c => !_digitOrDotRegex.Match(c.ToString()).Success);
    }

    private static IEnumerable<T> GetNeighbours<T>(int i, int j, T[][] matrix)
    {
        // Assume i and j are not on the edges of the matrix because I am lazy
        var upper = GetUpperNeighbours(i, j, matrix);
        var lower = GetLowerNeighbours(i, j, matrix);
        var left = GetLeftNeighbours(i, j, matrix);
        var right = GetRightNeighbours(i, j, matrix);

        // return upper.Concat(lower).Append(left).Append(right);
        foreach (var item in upper)
        {
            yield return item;
        }
        yield return left;
        yield return right;
        foreach (var item in lower)
        {
            yield return item;
        }
    }

    private static T GetRightNeighbours<T>(int i, int j, T[][] matrix) => matrix[i][j + 1];

    private static T GetLeftNeighbours<T>(int i, int j, T[][] matrix) => matrix[i][j - 1];

    private static T[] GetLowerNeighbours<T>(int i, int j, T[][] matrix) => matrix[i + 1][(j - 1)..(j + 2)];

    private static T[] GetUpperNeighbours<T>(int i, int j, T[][] matrix) => matrix[i - 1][(j - 1)..(j + 2)];

    private static char[][] GetPaddedInput(StreamReader streamReader)
    {
        var inputPadded = new List<char[]>();

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            inputPadded.Add($".{line}.".ToCharArray());
        }

        var lineLength = inputPadded[0].Length;
        var paddingLine = new char[lineLength];
        Array.Fill(paddingLine, '.');
        inputPadded.Insert(0, paddingLine);
        inputPadded.Add(paddingLine);

        return inputPadded.ToArray();
    }
}