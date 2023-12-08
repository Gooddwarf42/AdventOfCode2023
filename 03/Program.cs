using System.Text.RegularExpressions;

internal class Program
{
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

        var notDigitRegex = new Regex(@"\D");
        var digitOrDotRegex = new Regex(@"\d|.");

        var listOfGoodNumbers = new List<int>();
        var parsingNumState = false;
        var isGoodNumber = false;

        for (int i = 1; i < inputPadded.Length - 1; i++)
        {
            var row = inputPadded[i];
            for (int j = 1; j < row.Length - 1; j++)
            {
                var currentChar = row[j];
                if (notDigitRegex.Match(currentChar.ToString()).Success)
                {
                    if (!parsingNumState || !isGoodNumber)
                    {
                        continue;
                    }

                    // finished parsing a good number, add it to the good list of numbers


                    parsingNumState = false;
                    isGoodNumber = false;
                }

                // current character is a digit. 

                // Initialize parsing if needed
                if (!parsingNumState)
                {
                    parsingNumState = true;
                }

                // Current parse already confirmed number to be a good boy
                if (isGoodNumber)
                {
                    continue;
                }

                // Check if current digit is good and act accordingly
                if (IsDigitGood(i, j, inputPadded))
                {
                    // Handle here keeping track of good digits
                    isGoodNumber = true;
                }

            }
        }
    }

    private static bool IsDigitGood(int i, int j, char[][] inputPadded)
    {
        var neighbours = GetNeighbours(i, j, inputPadded);

        System.Console.WriteLine($"printing neighbourrs of {i}, {j}");
        foreach (var mimmo in neighbours){
            System.Console.WriteLine(mimmo);
        }


        throw new NotImplementedException();
    }

    private static IEnumerable<T> GetNeighbours<T>(int i, int j, T[][] matrix)
    {
        // Assume i and j are not on the edges of the matrix because I am lazy
        var upper = matrix[i - 1][(j - 1)..(j + 2)];
        var lower = matrix[i + 1][(j - 1)..(j + 2)];
        var left = matrix[i][j - 1];
        var right = matrix[i][j + 1];

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