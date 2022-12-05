using System.Text.RegularExpressions;
using SuperLinq;

namespace AdventOfCode2022;

public class Day5 : ISolver
{
    public (string, string) ExpectedResult => ("WHTLRMZRC", "");

    public (string, string) Solve(string[] input)
    {
        return (Part1(ParseStartingState(input), input), Part2(ParseStartingState(input), input));
    }

    Stack<char>[] ParseStartingState(string[] input)
    {
        var stacks = Enumerable.Range(1, (input[0].Length+1)/4).Select(_ => new Stack<char>()).ToArray();
        foreach (var line in input.TakeWhile(s => s.Contains('[')))
        {
            for (var n = 0; n < stacks.Length; n++)
            {
                var c = line[1 + n * 4];
                if (c != ' ')
                {
                    stacks[n].Push(c);
                }
            }
        }
        for (var n = 0; n < stacks.Length; n++)
        {
            // reverse each one
            stacks[n] = new Stack<char>(stacks[n]);
        }
        return stacks;
    }

    string Part1(Stack<char>[] state, IEnumerable<string> input)
    {
        var regex = new Regex(@"move (\d+) from (\d+) to (\d+)");
        foreach (var instruction in input.Select(n => regex.Match(n))
                .Where(m => m.Success)
                .Select(m => new { Amount = int.Parse(m.Groups[1].Value), From = int.Parse(m.Groups[2].Value), To = int.Parse(m.Groups[3].Value) }))
        {
            Console.WriteLine($"move {instruction.Amount} from {instruction.From} to {instruction.To}");
            for (var n = 0; n < instruction.Amount; n++)
            {
                state[instruction.To - 1].Push(state[instruction.From - 1].Pop());
            }
        }
        return string.Concat(state.Select(s => s.Pop()));
    }

    string Part2(Stack<char>[] startingState, IEnumerable<string> input) => "";

}
