namespace AdventOfCode2022;

public class Day8 : ISolver
{
    public (string, string) ExpectedResult => ("", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public long Part1(string[] input)
    {
        var g = Grid<int>.ParseToGrid(input);
        var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        return g.AllPositions().Count(p =>  dirs.Any(d => g.LineOut(p,d).All(n => g[p] > g[n])));
    }
    public long Part2(IEnumerable<string> input) => 0;

}
