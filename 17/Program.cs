using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();

        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);

        var map = ParseInput(streamReader).ToArray();

        


        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");
        System.Console.WriteLine(42);
    }
    private static IEnumerable<int[]> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            yield return streamReader.ReadLine()!
                .Split(null)
                .Select(c => int.Parse(c))
                .ToArray();
        }
    }
}