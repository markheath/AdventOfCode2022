using SuperLinq;

namespace AdventOfCode2022;

// https://adventofcode.com/2022/day/1
public class Day1 : ISolver
{
    public (string, string) ExpectedResult => ("70698", "206643");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    long Part1(IEnumerable<string> input) =>
            input.GroupAdjacent(s => s.Length > 0)
                .Where(g => g.Key)
                .Select(g => g.Sum(n => int.Parse(n)))
                .Max();
    long Part2(IEnumerable<string> input) =>
            input.GroupAdjacent(s => s.Length > 0)
                .Where(g => g.Key)
                .Select(g => g.Sum(n => int.Parse(n)))
                .OrderByDescending(n => n)
                .Take(3)
                .Sum();

}
