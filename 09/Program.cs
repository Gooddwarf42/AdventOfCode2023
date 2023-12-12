// See https://aka.ms/new-console-template for more information

internal class Program
{
    private static void Main(string[] args)
    {
        var inputPath = $"input.txt";
        using var streamReader = new StreamReader(inputPath);

        var total = 0L;
        foreach (var sequence in ParseInput(streamReader))
        {
            var nextNumber = ComputeNext(sequence);
            System.Console.WriteLine($"To extend the sequence you need {nextNumber}");
            total += nextNumber;
        }
        System.Console.WriteLine($"The solution of the problem is {total}");
        MathUtilities.PrintComputedBinoms();
    }

    private static long ComputeNext(int[] sequence)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<int[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            yield return line!.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(st => int.Parse(st)).ToArray();
        }
    }

}


internal static class MathUtilities
{
    private static List<long[]> _binom = [[1]];
    public static long Binom(int n, int k)
    {
        var meaningfulEntries = (int)Math.Ceiling((n + 1) / 2.0);

        if (k < 0 || k > n)
        {
            return 0;
        }

        if (k >= meaningfulEntries)
        {
            return Binom(n, n - k);
        }

        if (BinomAlreadyComputed(n, k))
        {
            return _binom[n][k];
        }

        var result = Binom(n - 1, k - 1) + Binom(n - 1, k);

        // if previous rows are needed they were computed by previous recursive calls!
        // this is equivalent to _binom.Count == n
        if (_binom.Count <= n)
        {
            _binom.Add(new long[meaningfulEntries]);
        }
        _binom[n][k] = result;
        return result;
    }

    public static void PrintComputedBinoms()
    {
        foreach (var row in _binom)
        {
            System.Console.WriteLine(string.Join(' ', row));
        }
    }

    private static bool BinomAlreadyComputed(int n, int k) =>
        _binom.Count > n
        && _binom[n][k] != 0;
}