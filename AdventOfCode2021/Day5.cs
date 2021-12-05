using System.Text.RegularExpressions;

internal class Day5
{
    internal static async Task RunFirst()
    {
        var regex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)", RegexOptions.Compiled);
        var text = await File.ReadAllTextAsync("day5.txt");
        var dangerousPoints = regex
            .Matches(text)
            .Select(m => 
            {
                var ax = int.Parse(m.Groups[1].Value);
                var ay = int.Parse(m.Groups[2].Value);
                var bx = int.Parse(m.Groups[3].Value);
                var by = int.Parse(m.Groups[4].Value);

                return new Line(new(ax,ay), new(bx,by));
            })
            .Select(l => l switch
            {
                { Vertical: true, Horizontal: true } => new[] { l.A },
                { Vertical: true} => Enumerable
                    .Range(Math.Min(l.A.Y, l.B.Y), Math.Abs(l.A.Y - l.B.Y) + 1)
                    .Select(y => new Point(l.A.X, y)),
                { Horizontal: true } => Enumerable
                    .Range(Math.Min(l.A.X, l.B.X), Math.Abs(l.A.X - l.B.X) + 1)
                    .Select(x => new Point(x, l.A.Y)),
                _ => Array.Empty<Point>()
            })
            .SelectMany(x => x)
            .GroupBy(x => x)
            .Select(g => (point: g.Key, score: g.Count()))
            .Count(p => p.score > 1)
            ;

        Console.WriteLine($"{dangerousPoints} dangerous places");
    }

    internal static async Task RunSecond()
    {
        var regex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)", RegexOptions.Compiled);
        var text = await File.ReadAllTextAsync("day5.txt");
        var dangerousPoints = regex
            .Matches(text)
            .Select(m => 
            {
                var ax = int.Parse(m.Groups[1].Value);
                var ay = int.Parse(m.Groups[2].Value);
                var bx = int.Parse(m.Groups[3].Value);
                var by = int.Parse(m.Groups[4].Value);

                return new Line(new(ax,ay), new(bx,by));
            })
            .Select(l => l switch
            {
                { Vertical: true, Horizontal: true } => new[] { l.A },
                { Vertical: true} => Enumerable
                    .Range(Math.Min(l.A.Y, l.B.Y), Math.Abs(l.A.Y - l.B.Y) + 1)
                    .Select(y => new Point(l.A.X, y)),
                { Horizontal: true } => Enumerable
                    .Range(Math.Min(l.A.X, l.B.X), Math.Abs(l.A.X - l.B.X) + 1)
                    .Select(x => new Point(x, l.A.Y)),
                _ => DevelopDiagonalLine(l)
            })
            .SelectMany(x => x)
            .GroupBy(x => x)
            .Select(g => (point: g.Key, score: g.Count()))
            .Count(p => p.score > 1)
            ;

        Console.WriteLine($"{dangerousPoints} dangerous places");
    }

    private static IEnumerable<Point> DevelopDiagonalLine(Line l)
    {
        var horizontalDelta = l.A.X > l.B.X ? -1 : 1;
        var verticalDelta = l.A.Y > l.B.Y ? -1 : 1;
        var n = Math.Abs(l.A.X - l.B.X) + 1;
        var points = Enumerable
            .Range(0, n)
            .Select(c => new Point(l.A.X + (horizontalDelta * c), l.A.Y + (verticalDelta * c)));

        return points;
    }

    internal record Point(int X, int Y);
    internal record Line(Point A, Point B)
    {
        public bool Vertical => A.X == B.X;
        public bool Horizontal => A.Y == B.Y;
    }
}