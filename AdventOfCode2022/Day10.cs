using System.Runtime.CompilerServices;

namespace AdventOfCode2022;

public class Day10 : ISolver
{
    
    public (string, string) ExpectedResult => ("14060", "");

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
        /*var X = 1;
        var cycle = 1;
        long signalStrength = 0;
        var targetPos = 0;
        foreach(var inst in input)
        {
            if (cycle >= targets[targetPos])
            {
                signalStrength += X * targets[targetPos];
                targetPos++;
                if (targetPos >= targets.Length) break;
            }

            if (inst == "noop")
            {
                cycle ++;
            }
            if (inst.StartsWith("addx"))
            {
                X += int.Parse(inst[5..]);
                cycle += 2;
            }
        }
        while(targetPos < targets.Length)
        {
            signalStrength += X * targets[targetPos];
            targetPos++;
        }
        return signalStrength;*/
    }
    long Part2(IEnumerable<string> input) => 0;

}
