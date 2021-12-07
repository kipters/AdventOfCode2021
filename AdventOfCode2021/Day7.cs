internal class Day7
{
    internal static async Task RunFirst()
    {
        var data = await File.ReadAllTextAsync("day7.txt");
        var positions = data
            .Split(',')
            .Select(int.Parse)
            .ToArray();
        
        var max = positions.Max();

        var minimumCost = Enumerable
            .Range(0, max)
            .Select(t => positions
                .Select(p => Math.Abs(p - t))
                .Sum()
            )
            .Min();

        Console.WriteLine($"Minimum fuel expense: {minimumCost} units");
    }

    internal static async Task RunSecond()
    {
        var data = await File.ReadAllTextAsync("day7.txt");
        var positions = data
            .Split(',')
            .Select(int.Parse)
            .ToArray();
        
        var max = positions.Max();

        var costCache = new Dictionary<int, int>();

        var minimumCost = Enumerable
            .Range(0, max)
            .Select(t => positions
                .Select(p =>
                {
                    var distance = Math.Abs(p - t);
                    if (!costCache.TryGetValue(distance, out var additionalCost))
                    {
                        additionalCost = Enumerable.Range(0, distance).Sum();
                        costCache[distance] = additionalCost;
                    }
                    return distance + additionalCost;
                })
                .Sum()
            )
            .Min();

        Console.WriteLine($"Minimum fuel expense: {minimumCost} units");
    }
}