// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

internal class Program
{
    public static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();

        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);
        var input = streamReader.ReadLine();

        var sumOfHashes = SumHashes(input!);
        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");

        System.Console.WriteLine(sumOfHashes);
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
}