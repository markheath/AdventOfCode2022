using System.Text.RegularExpressions;
using SuperLinq;

namespace AdventOfCode2022;

public class Day5 : ISolver
{
    public (string, string) ExpectedResult => ("WHTLRMZRC", "GMPMLWNMG");

    public (string, string) Solve(string[] input)
    {
        return (Solve(ParseStartingState(input), input,1), Solve(ParseStartingState(input), input,2));
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

    string Solve(Stack<char>[] state, IEnumerable<string> input, int part)
    {
        var regex = new Regex(@"move (\d+) from (\d+) to (\d+)");
        foreach (var instruction in input.Select(n => regex.Match(n))
                .Where(m => m.Success)
                .Select(m => new { Amount = int.Parse(m.Groups[1].Value), From = int.Parse(m.Groups[2].Value), To = int.Parse(m.Groups[3].Value) }))
        {
            //Console.WriteLine($"move {instruction.Amount} from {instruction.From} to {instruction.To}");
            if (part == 1)
                Enumerable.Range(1, instruction.Amount).ForEach(n => state[instruction.To - 1].Push(state[instruction.From - 1].Pop()));
            else
                Enumerable.Range(1, instruction.Amount).Select(n => state[instruction.From - 1].Pop()).Reverse().ForEach(n => state[instruction.To - 1].Push(n));
        }
        return string.Concat(state.Select(s => s.Pop()));
    }

}
