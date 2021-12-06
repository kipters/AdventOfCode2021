internal class Day6
{
    internal static async Task RunFirst()
    {
        var initialPopulation = await ParsePopulationFile("day6.txt");
        var finalPopulationSize = FastGetPopulation(initialPopulation, 80);

        Console.WriteLine($"{finalPopulationSize} lantern fishes after 80 days");
    }

    private static long FastGetPopulation(List<byte> initialPopulation, int generations)
    {
        var growthRates = Enumerable.Range(0, 7)
            .Select(x => (long) initialPopulation.Count(i => i == x))
            .ToArray();

        var teens = new long[9];

        long population = initialPopulation.Count;

        for (int i = 0; i < generations; i++)
        {
            var dayOfWeek = i % 7;
            var dayOfMaturationCycle = i % 9;
            
            var newBorns = growthRates[dayOfWeek];
            var firstBorns = teens[dayOfMaturationCycle];
            teens[dayOfMaturationCycle] = newBorns + firstBorns;
            growthRates[dayOfWeek] += firstBorns;

            population += newBorns;
            population += firstBorns;
        }

        return population;
    }

    internal static async Task RunSecond()
    {
        var initialPopulation = await ParsePopulationFile("day6.txt");
        var finalPopulationSize = FastGetPopulation(initialPopulation, 256);

        Console.WriteLine($"{finalPopulationSize} lantern fishes after 256 days");
    }

    private static async Task<List<byte>> ParsePopulationFile(string filename)
    {
        var line = await File.ReadAllTextAsync(filename);
        var initialPopulation = line
            .Split(',')
            .Select(byte.Parse)
            .ToList();
        return initialPopulation;
    }

    // Do not use, this is O(scary), I only left it as a testament of my first naive attempt at this
    private static long GetPopulation(List<byte> initialPopulation, int generations)
    {
        return Enumerable
            .Range(0, generations)
            .Aggregate(
                initialPopulation,
                (p, _) =>
                {
                    var px = p
                        .AsParallel()
                        .Select(f => f switch
                        {
                            0 => new byte[] { 6, 8 },
                            _ => new byte[] { (byte)(f - 1) }
                        })
                        .SelectMany(_ => _)
                        .ToList();

                    var matured = px.Count(n => n == 8);
                    Console.WriteLine($"Gen {_}: {p.Count} ({matured})");
                    return px;
                },
                p => p.Count
            );
    }
}