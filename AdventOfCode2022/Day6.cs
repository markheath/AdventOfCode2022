using SuperLinq;

namespace AdventOfCode2022;

public class Day6 : ISolver
{
    public (string, string) ExpectedResult => ("1892", "2313");

    public int FindMarkerPosition(string input, int distinct) =>
        input.Window(distinct).TakeUntil(w => w.ToHashSet().Count == distinct).Count() + distinct - 1;

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    long Part1(IEnumerable<string> input) => FindMarkerPosition(input.First(), 4);
    long Part2(IEnumerable<string> input) => FindMarkerPosition(input.First(), 14);

}
