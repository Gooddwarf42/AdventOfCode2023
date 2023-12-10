using System.ComponentModel;
using System.Text.RegularExpressions;

internal class Program
{
    private static readonly Regex NumberRegex = new(@"\d+");

    private static void Main(string[] args)
    {
        const int day = 7;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        var hands = ParseInput(streamReader).ToArray();

        var sortedHands = hands
            .OrderBy(h => (int)h.Type)
            .ThenBy(h => h.ConvenientlyReplacedHand)
            .ToArray();

        var result = 0;
        for (int i = 0; i < hands.Length; i++)
        {
            result += (i + 1) * sortedHands[i].Bid;
        }

        System.Console.WriteLine(result);
    }

    private static IEnumerable<Hand> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            var split = line!.Split(' ');
            yield return new Hand
            {
                RawHand = split[0],
                Bid = int.Parse(split[1]),
            };
        }
    }
}

internal class Hand
{
    private static readonly Dictionary<char, char> VeryConvenientMapDict = new()
    {
        {'T', 'a'},
        {'J', 'b'},
        {'Q', 'c'},
        {'K', 'd'},
        {'A', 'e'},
    };
    public required string RawHand { get; init; }
    public required int Bid { get; init; }
    public string ConvenientlyReplacedHand
    {
        get
        {
            var substitutedEnumerable = RawHand.ToArray()
                .Select(VeryConvenientMap);
            return string.Join(null, substitutedEnumerable);
        }
    }

    public HandType Type
    {
        get
        {
            var groupedHand = RawHand.ToArray()
                .GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .ToArray();

            return groupedHand.Length switch
            {
                1 => HandType.FiveEquals,
                2 => groupedHand[0].Count() == 4
                        ? HandType.Poker
                        : HandType.Full,
                3 => groupedHand[0].Count() == 3
                        ? HandType.Tris
                        : HandType.DoublePair,
                4 => HandType.Pair,
                5 => HandType.HighCard,
                _ => throw new Exception("ma che razza di mano è??"),
            };
        }
    }

    private static char VeryConvenientMap(char input)
    {
        if (VeryConvenientMapDict.TryGetValue(input, out var output))
        {
            return output;
        }
        // In this case the char is (hopefully) a digit, so we leave it alone.
        return input;
    }

}
internal enum HandType
{
    HighCard = 0,
    Pair = 1,
    DoublePair = 2,
    Tris = 3,
    Full = 4,
    Poker = 5,
    FiveEquals = 6,
}