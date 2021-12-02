internal class Day1
{
    internal static async Task RunFirst()
    {
        var lines = await File.ReadAllLinesAsync("day1.txt");
        var numbers = lines
            .Select(long.Parse);

        var count = numbers
            .Skip(1)
            .Aggregate(
                (count: 0, last: numbers.First()),
                (acc, n) => (n > acc.last ? acc.count + 1 : acc.count, n),
                acc => acc.count
            );

        Console.WriteLine($"{count} measurements larger than the previous");
    }

    internal static async Task RunSecond()
    {
        var lines = await File.ReadAllLinesAsync("day1.txt");
        var numbers = lines
            .Select(long.Parse)
            .SlidingWindow(3)
            .Select(w => w.Sum());

        var count = numbers
            .Skip(1)
            .Aggregate(
                (count: 0, last: numbers.First()),
                (acc, n) => (n > acc.last ? acc.count + 1 : acc.count, n),
                acc => acc.count
            );

        Console.WriteLine($"{count} window measurements larger than the previous");
    }
}