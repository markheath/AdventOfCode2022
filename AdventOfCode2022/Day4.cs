namespace AdventOfCode2022;

public class Day4 : ISolver
{
    public (string, string) ExpectedResult => ("450", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    private bool Contains(int[] numbers)
    {
        return (numbers[0] >= numbers[2] && numbers[1] <= numbers[3]) ||
            (numbers[0] <= numbers[2] && numbers[1] >= numbers[3]);
    }

    long Part1(IEnumerable<string> input) =>
        input.Select(n => n.Split(',', '-').Select(int.Parse).ToArray())
        .Count(n => Contains(n));
    long Part2(IEnumerable<string> input) => 0;

}
