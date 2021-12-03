internal class Day3
{
    internal static async Task RunFirst()
    {
        var lines = await File.ReadAllLinesAsync("day3.txt");
        var lineLen = lines[0].Length;

        var gammaRate = lines
            .Select(x => x.ToCharArray())
            .Aggregate(
                new int[lineLen],
                (acc, l) => l
                    .Zip(acc, (c, s) => c switch
                    {
                        '1' => s + 1,
                        '0' => s - 1,
                        _ => throw new Exception($"Invalid char {c}")
                    })
                    .ToArray(),
                a => a
                    .Select(n => n > 0 ? '1' : '0')
                    .Stringify()
                    .Apply(x => Convert.ToInt32(x, 2))
            );

        var epsilonRate = (ushort) ~(gammaRate | (Int32.MaxValue << lineLen));

        var powerConsumption = gammaRate * epsilonRate;

        Console.WriteLine($"Gamma: {gammaRate}, Epsilon: {epsilonRate}, Power: {powerConsumption}");
    }

    internal static async Task RunSecond()
    {
        var lines = await File.ReadAllLinesAsync("day3.txt");
        var lineLen = lines[0].Length;

        var oxygenRating = FindRating((one, zero) => one >= zero ? '1' : '0');
        var scrubberRating = FindRating((one, zero) => zero <= one ? '0' : '1');

        Console.WriteLine($"Life support rating: {oxygenRating * scrubberRating}");

        int FindRating(Func<int, int, char> predicate)
        {
            var partial = string.Empty;

            do
            {
                var subset = lines!
                    .Where(l => l.StartsWith(partial))
                    .ToArray();

                if (subset.Length == 1)
                {
                    return Convert.ToInt32(subset[0], 2);
                }

                var next = subset
                    .Select(l => l[partial.Length])
                    .GroupBy(_ => _)
                    .ToDictionary(x => x.Key, x => x.Count())
                    .Apply(d => predicate(
                        d.TryGetValue('1', out var one) ? one : 0, 
                        d.TryGetValue('0', out var zero) ? zero : 0)
                    )
                    ;

                partial += next;
                
            } while (partial.Length < lineLen);

            var rating = Convert.ToInt32(partial, 2);

            return rating;
        }
    }
}