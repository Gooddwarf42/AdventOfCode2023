internal class Program
{
    private static void Main(string[] args)
    {
        const int day = 3;
        var inputPath = $"/home/marco/share/AdventOfCode2023/{day:D2}/input";
        using var streamReader = new StreamReader(inputPath);

        var inputPadded = GetPaddedInput(streamReader);

        // foreach (var line in inputPadded)
        // {
        //     Console.WriteLine(string.Join(null, line));
        // }

    }

    private static char[][] GetPaddedInput(StreamReader streamReader)
    {
        var inputPadded = new List<char[]>();

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine()!;
            inputPadded.Add($".{line}.".ToCharArray());
        }

        var lineLength = inputPadded[0].Length;
        var paddingLine = new char[lineLength];
        Array.Fill(paddingLine, '.');
        inputPadded.Insert(0, paddingLine);
        inputPadded.Add(paddingLine);

        return inputPadded.ToArray();
    }
}