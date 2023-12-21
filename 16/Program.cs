// See https://aka.ms/new-console-template for more information

using System.Collections.Specialized;
using System.Diagnostics;

internal class Program
{
    public static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();

        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);

        var inputSequence = ParseInput(streamReader).ToArray();

        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");

        System.Console.WriteLine(42);
    }

    private static char[][] ParseInput(StreamReader streamReader)
    {
        throw new NotImplementedException();
    }
}