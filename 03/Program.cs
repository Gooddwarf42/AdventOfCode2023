using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NotDigitRegex = new Regex(@"\D");
    private static readonly Regex DigitOrDotRegex = new Regex(@"\d|\.");
    private static readonly Regex NumberRegex = new Regex(@"\d+");
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
        System.Console.WriteLine($"Part 1: {total}");

        var total2 = SolvePart2(inputPadded);
        System.Console.WriteLine($"Part 2: {total2}");
    }
    private static int SolvePart2(char[][] inputPadded)
    {
        var total = 0;
        for (int i = 1; i < inputPadded.Length - 1; i++)
        {
            var row = inputPadded[i];
            for (int j = 1; j < row.Length - 1; j++)
            {
                var currentChar = row[j];

                if (currentChar != '*')
                {
                    continue;
                }

                var surroundingNumbers = GetSurroundingNumbers(i, j, inputPadded);

                if (surroundingNumbers.Count == 2)
                {
                    total += surroundingNumbers[0] * surroundingNumbers[1];
                }
            }
        }

        return total;
    }

    private static List<int> GetSurroundingNumbers(int i, int j, char[][] inputPadded)
    {
        var numbersList = new List<int>();
        // Once again, we assume the padding on all sides.

        string trimmedRowAbove = TrimRowToTheInterestingPart(i - 1);
        string trimmedRowMiddle = TrimRowToTheInterestingPart(i);
        string trimmedRowBelow = TrimRowToTheInterestingPart(i + 1);

        numbersList.AddRange(GetNumbersInString(trimmedRowAbove));
        numbersList.AddRange(GetNumbersInString(trimmedRowMiddle));
        numbersList.AddRange(GetNumbersInString(trimmedRowBelow));

        return numbersList;

        string TrimRowToTheInterestingPart(int rowIndex)
        {
            var row = string.Join(null, inputPadded[rowIndex]);
            var leftPart = row[..j];
            var rightPart = row[(j + 1)..];
            var leftTrimPoint = NotDigitRegex.Matches(leftPart).Last().Index;
            var rightTrimPoint = j + 1 + NotDigitRegex.Matches(rightPart).First().Index;
            var trimmedRow = row[leftTrimPoint..(rightTrimPoint + 1)];

            if (trimmedRow.Length < 3)
            {
                throw new Exception("I am getting too sleepy for all these indices shenanigans");
            }

            return trimmedRow;
        }

        static IEnumerable<int> GetNumbersInString(string str)
            => NumberRegex.Matches(str)
                .Select(m => int.Parse(m.Value));
    }

    private static int SolvePart1(char[][] inputPadded)
    {
        var total = 0;

        var currentNumberStringBuilder = new StringBuilder();

        var parsingNumState = false;
        var isGoodNumber = false;

        for (int i = 1; i < inputPadded.Length - 1; i++)
        {
            var row = inputPadded[i];
            for (int j = 1; j < row.Length - 1; j++)
            {
                var currentChar = row[j];
                if (NotDigitRegex.Match(currentChar.ToString()).Success)
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

        return neighbours.Any(c => !DigitOrDotRegex.Match(c.ToString()).Success);
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