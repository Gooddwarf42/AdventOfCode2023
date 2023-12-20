// See https://aka.ms/new-console-template for more information

using System.Collections.Specialized;
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
        var boxes = new OrderedDictionary?[256];
        PlaceLensesInBoxes(inputSequence, boxes);
        var sumOfFocusingPower = ComputeSumOfFocusingPower(boxes);
        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");

        System.Console.WriteLine(sumOfFocusingPower);
    }

    private static void PlaceLensesInBoxes(SequenceElement[] inputSequence, OrderedDictionary?[] boxes)
    {
        foreach (var sequenceElement in inputSequence)
        {
            sequenceElement.Apply(boxes);
        }
    }

    private static int ComputeSumOfFocusingPower(OrderedDictionary?[] boxes)
    {
        var sum = 0;
        for (var i = 0; i < boxes.Length; i++)
        {
            var box = boxes[i];
            if (box is null)
            {
                continue;
            }

            for (var j = 0; j < box.Count; j++)
            {
                sum += (i + 1) * (j + 1) * (int)box[j]!;
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
            var match = sequenceElementRegex.Match(rawSequenceElement);
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
        private int LabelHash => ComputeHash(Label);

        public void Apply(OrderedDictionary?[] boxes)
        {
            // assume boxes hass length 256
            switch (Operand)
            {
                case '=':
                    boxes[LabelHash] ??= new OrderedDictionary();
                    boxes[LabelHash]![Label] = FocalLength!.Value;
                    break;
                case '-':
                    if (boxes[LabelHash]?.Contains(Label) is true)
                    {
                        boxes[LabelHash]!.Remove(Label);
                    }

                    break;
                default:
                    throw new Exception("InvalidOperand");
            }
        }
    }
}