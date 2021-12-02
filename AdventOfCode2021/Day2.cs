using System.Text.RegularExpressions;

internal class Day2
{
    internal static async Task RunFirst()
    {
        var lines = await File.ReadAllTextAsync("day2.txt");
        var (position, depth) = Regex.Matches(lines, @"([a-z]+) (\d+)")
            .Select(m => (cmd: m.Groups[1].Value.Trim(), amt: int.Parse(m.Groups[2].Value)))
            .Aggregate(
                (x: 0, y: 0),
                (acc, p) => p.cmd switch
                {
                    "forward" => (acc.x + p.amt, acc.y),
                    "down" => (acc.x, acc.y + p.amt),
                    "up" => (acc.x, acc.y - p.amt),
                    _ => throw new Exception($"Invalid command {p}")
                },
                a => a
            );

        Console.WriteLine($"Final position product: {position * depth}");
    }

    internal static async Task RunSecond()
    {
        var lines = await File.ReadAllTextAsync("day2.txt");
        var (position, depth, _) = Regex.Matches(lines, @"([a-z]+) (\d+)")
            .Select(m => (cmd: m.Groups[1].Value.Trim(), amt: int.Parse(m.Groups[2].Value)))
            .Aggregate(
                (x: 0, y: 0, a: 0),
                (acc, cmd) => cmd.cmd switch
                {
                    "down" => (acc.x, acc.y, acc.a + cmd.amt),
                    "up" => (acc.x, acc.y, acc.a - cmd.amt),
                    "forward" => (acc.x + cmd.amt, acc.y + (acc.a * cmd.amt), acc.a),
                    _ => throw new Exception($"Invalid command {cmd}")
                },
                a => a
            );

        Console.WriteLine($"Final position product: {position * depth}");
    }
}