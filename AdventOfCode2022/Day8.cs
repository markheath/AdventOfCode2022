using SuperLinq;

namespace AdventOfCode2022;

public class Day8 : ISolver
{
    public (string, string) ExpectedResult => ("1832", "157320");

    private static readonly Coord[] dirs = new Coord[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

    public (string, string) Solve(string[] input)
    {
        var g = Grid<int>.ParseToGrid(input);        
        return ($"{Part1(g)}", $"{Part2(g)}");
    }

    private long Part1(Grid<int> g)
            => g.AllPositions().Count(p =>  dirs.Any(d => g.LineOut(p,d).All(n => g[p] > g[n])));


    public long ScenicScore(Grid<int> g, Coord p)
        => dirs.Select(d => g.LineOut(p, d)
                            .TakeUntil(n => g[p] <= g[n]).Count())
               .Aggregate((a, b) => a * b);

    public long Part2(Grid<int> g)
        => g.AllPositions().Max(p => ScenicScore(g, p));
}
