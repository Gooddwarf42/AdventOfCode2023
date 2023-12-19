// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.RegularExpressions;

internal class Program
{
    public static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();

        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);
        var input = streamReader.ReadLine();

        var inputSequence = ParseInput(input!).ToArray();
        var boxes = new Dictionary<string, int>?[256];
        PlaceLensesInBoxes(inputSequence, boxes);
        var sumOfFocusingPower = ComputeSumOfFocusingPower(boxes);
        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");

        System.Console.WriteLine(42);
    }

    private static void PlaceLensesInBoxes(SequenceElement[] inputSequence, Dictionary<string, int>?[] boxes)
    {
        throw new NotImplementedException();
    }

    private static object ComputeSumOfFocusingPower(Dictionary<string, int>?[] boxes)
    {
        var sum = 0;
        for (var i = 0; i < boxes.Length; i++)
        {
            var box = boxes[i]?.ToList();
            if (box is null)
            {
                continue;
            }

            for (var j = 0; j < box.Count; j++)
            {
                var lens = box[i];
                sum += (i + 1) * (j + 1) * lens.Value;
            }
        }

        return sum;
    }

    private static IEnumerable<SequenceElement> ParseInput(string input)
    {
        var sequenceElements = input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var sequenceElementRegex = new Regex(@"(?<label>([a-z]|[A-Z])+)(?<operand>\=|\-)(?<focal>\d?)");

        foreach (var rawSequenceElement in sequenceElements)
        {
            var match = sequenceElementRegex.Match(rawSequenceElement)!;
            if (!match.Success)
            {
                throw new Exception("Mimmo la regex!");
            }

            yield return new SequenceElement()
            {
                Label = match.Groups["label"].Value,
                Operand = match.Groups["operand"].Value[0],
                FocalLength = string.IsNullOrWhiteSpace(match.Groups["focal"].Value) ? null : int.Parse(match.Groups["focal"].Value),
            };
        }
    }

    private static long SumHashes(string input)
    {
        var sequenceElements = input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var hashes = sequenceElements.Select(ComputeHash);

        return hashes.Sum(i => (long)i);
    }

    private static int ComputeHash(string s)
    {
        var hash = 0;
        foreach (var character in s)
        {
            hash += (int)character;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }

    private sealed class SequenceElement
    {
        public required string Label { get; init; }
        public required char Operand { get; init; }
        public int? FocalLength { get; init; }
        public int LabelHash => ComputeHash(Label);
    }
}