// Ugliest solution yet. BY FAR.
internal class Day4
{
    internal static async Task RunFirst()
    {
        var lines = await File.ReadAllLinesAsync("day4.txt");
        var drawSequence = ParseDrawSequence(lines);
        var boards = ParseBoards(lines);

        var winningRound = boards
            .ParamTransform(drawSequence, (board, drawn) => board
                .Select(row => row
                    .Select(column => (column.number, column.marked | column.number == drawn))
                    .ToArray()
                )
                .ToArray())
            .Select(round => (drawn: round.param, winners: round.intermediate.Where(IsWinner)))
            .First(r => r.winners.Any())
            ;

        var lastDrawn = winningRound.drawn;
        var winnerBoard = winningRound.winners.First();

        var flatBoard = winnerBoard
            .SelectMany(x => x)
            .ToArray();

        var unmarkedSum = flatBoard
            .Where(x => x.marked == false)
            .Select(x => x.number)
            .Sum();

        Console.WriteLine($"Final score: {unmarkedSum * lastDrawn}");
    }

    internal static async Task RunSecond()
    {
        var lines = await File.ReadAllLinesAsync("day4.txt");
        var drawSequence = ParseDrawSequence(lines);
        var boards = ParseBoards(lines);

        var lastWinningRound = boards
            .ParamTransform(drawSequence, (board, drawn) => board
                .Select(row => row
                    .Select(column => (column.number, column.marked | column.number == drawn))
                    .ToArray()
                )
                .ToArray())
            .Select(round => (drawn: round.param, winners: round.intermediate.Where(IsWinner)))
            .Aggregate(
                (winners: new HashSet<string>(), seq: new List<(int drawn, List<(int number, bool marked)[][]> winners)>()),
                (a, r) =>
                {
                    var newWinners = r.winners
                        .Where(x => !a.winners.Contains(GetBoardHash(x)))
                        .ToList();

                    foreach (var newWinner in newWinners)
                    {
                        a.winners.Add(GetBoardHash(newWinner));
                    }
                    
                    a.seq.Add((r.drawn, newWinners));

                    return a;
                },
                a => a.seq.Where(x => x.winners.Any()).Last()
            )

            ;

        var lastDrawn = lastWinningRound.drawn;
        var lastWinner = lastWinningRound.winners.Single();

        var flatBoard = lastWinner
            .SelectMany(x => x)
            .ToArray();

        var unmarkedSum = flatBoard
            .Where(x => x.marked == false)
            .Select(x => x.number)
            .Sum();

        Console.WriteLine($"Final score: {unmarkedSum * lastDrawn}");
    }

    private static string GetBoardHash((int number, bool _)[][] board) => 
        string.Join(null, board.SelectMany(_ => _).Select(_ => _.number));

    private static IEnumerable<(int number, bool marked)[][]> ParseBoards(string[] lines) => lines
        .Skip(2)
        .LineChunks()
        .Select((b, i) => b
            .Select(r => r
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(n => (number: int.Parse(n), marked: false))
                .ToArray()
            )
            .ToArray()
        )
        .ToArray();

    private static IEnumerable<int> ParseDrawSequence(string[] lines) => lines[0]
        .Split(',')
        .Select(int.Parse);

    internal static bool IsWinner((int number, bool marked)[][] b) => b
        .Any(r => r.All(c => c.marked)) ||
        Enumerable
            .Range(0, b[0].Length)
            .Select(i => b.Select(r => r[i]).ToArray())
            .Any(c => c.All(r => r.marked));
}