using System.Text;

namespace AdventOfCode2022;

public class Day10 : ISolver
{
    
    public (string, string) ExpectedResult => ("14060", "PAPKFKEJ");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public IEnumerable<int> Compute(IEnumerable<string> input)
    {
        int X = 1;
        foreach (var inst in input)
        {
            if (inst == "noop")
            {
                yield return X;
            }
            if (inst.StartsWith("addx"))
            {
                yield return X;
                yield return X;
                X += int.Parse(inst[5..]);
            }
        }
    }

    public long Part1(IEnumerable<string> input)
    {
        var output = Compute(input).ToArray();
        var targets = new[] { 20, 60, 100, 140, 180, 220 };
        return targets.Select(t => output[t - 1] * t).Sum();
    }
    long Part2(IEnumerable<string> input)
    {
        foreach(var c in Compute(input).Chunk(40))
        {
            var sb = new StringBuilder();
            for(int r = 0; r < c.Length; r++)
            {
                if (Math.Abs(c[r] - r) < 2)
                    sb.Append('#');
                else
                    sb.Append('.');
            }
            Console.WriteLine(sb);
        }
        return 0;
    }

}
