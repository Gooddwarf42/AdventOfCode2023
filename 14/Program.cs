using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();


        sw.Stop();
        System.Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");


    }
}