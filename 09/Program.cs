// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        var inputPath = $"input.txt";
        using var streamReader = new StreamReader(inputPath);

        System.Console.WriteLine($"Testing 50 choose 25: {MathUtilities.Binom(50, 25)}");

        MathUtilities.PrintComputedBinoms();
    }


}


internal static class MathUtilities
{
    private static List<long[]> _binom = [[1]];
    public static long Binom(int n, int k)
    {
        if (k < 0 || k > n)
        {
            return 0;
        }

        if (BinomAlreadyComputed(n, k))
        {
            return _binom[n][k];
        }

        var result = Binom(n - 1, k - 1) + Binom(n - 1, k);

        //if previous rows are needed they were computed by previous recursive calls!
        if (_binom.Count <= n)
        {
            _binom.Add(new long[n + 1]);
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