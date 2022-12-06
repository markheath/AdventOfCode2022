using SuperLinq;

namespace AdventOfCode2022;

public class Day6 : ISolver
{
    public (string, string) ExpectedResult => ("1892", "");

    public int FindMarkerPosition(string input) =>
        input.Window(4).TakeUntil(w => w.ToHashSet().Count == 4).Count() + 3;

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    long Part1(IEnumerable<string> input) => FindMarkerPosition(input.First());
    long Part2(IEnumerable<string> input) => 0;

}
