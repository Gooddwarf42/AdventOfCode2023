
using System.Linq.Expressions;

internal class Program
{
    private static void Main(string[] args)
    {
        const string inputPath = "input.txt";
        using var streamReader = new StreamReader(inputPath);
        var input = ParseInput(streamReader);

        foreach(var puzzle in input){
            System.Console.WriteLine(puzzle);
        }

    }

    private static IEnumerable<Picross> ParseInput(StreamReader streamReader)
    {
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            var lineSplit = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            // assume lineSplit has always two elements. If something goes horribly wrong I can throw here a panic exception, but lazy
            yield return new Picross
            {
                Schema = lineSplit[0].ToCharArray().ToList(),
                Constraints = lineSplit[1]
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(s => int.Parse(s))
                    .ToList(),
            };
        }
    }
}

internal sealed class Picross
{
    public required List<char> Schema { get; set; }
    public required List<int> Constraints { get; set; }

    public override string ToString()
    {
        var schema = string.Join(null, Schema);
        var constraints = string.Join(null, Constraints);
        return $"{schema} \t {constraints}";
    }
}
