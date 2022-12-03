namespace AdventOfCode2022;

public class Day3 : ISolver
{
    public (string, string) ExpectedResult => ("7831", "");

    public (string, string) Solve(string[] input)
    {
        return (Part1(input), Part2(input));
    }

    int Priority(char c) => c >= 'a' ? c - 'a' + 1 : c - 'A' + 27;

    string Part1(IEnumerable<string> input) => input
            .Select(r => (r[..(r.Length / 2)], r[(r.Length / 2)..]))
            .Select(a => Priority(a.Item1.Intersect(a.Item2).Single()))
            .Sum().ToString();
                   

    string Part2(IEnumerable<string> input) => input
            .Chunk(3)
            .Select(c => Priority(c[0].Intersect(c[1]).Intersect(c[2]).Single()))
            .Sum().ToString();

}
