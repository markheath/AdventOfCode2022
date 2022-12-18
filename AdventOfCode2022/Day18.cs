namespace AdventOfCode2022;

public class Day18 : ISolver
{
    public (string, string) ExpectedResult => ("", "");

    public (string, string) Solve(string[] input)
    {
        var pos = input.Select(line => line.Split(',').Select(int.Parse).ToArray()).Select(c => new Coord3(c[0], c[1], c[2])).ToHashSet();
        var part1 = pos.Sum(c => 6 - c.Neighbours().Count(n => pos.Contains(n)));
        
        return ($"{part1}", $"{0}");
    }

    long Part1(IEnumerable<string> input) => throw new NotImplementedException();
    long Part2(IEnumerable<string> input) => throw new NotImplementedException();

}
