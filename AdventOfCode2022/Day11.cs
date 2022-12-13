using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day11 : ISolver
{
    class Monkey
    {
        public int InspectCount { get; set; }
        public List<BigInteger> Items { get; } = new List<BigInteger>();
        public Func<BigInteger, BigInteger> Operation { get; set; } = n => n;
        public int DivisibleBy { get; set; }
        public int TrueTarget { get; set; }
        public int FalseTarget { get; set; }
    }

    public (string, string) ExpectedResult => ("182293", "54832778815");

    public (string, string) Solve(string[] input)
    {
        var monkeys = ParseMonkeys(input);

        // do 20 rounds
        for (var n = 0; n < 20; n++)
        {
            DoRound(monkeys, n => n / 3);
        }
        var part1 = monkeys.Select(m => (long)m.InspectCount).OrderDescending().Take(2).Aggregate((a, b) => a * b);

        // do 20 rounds
        monkeys = ParseMonkeys(input);
        var divisor = monkeys.Select(m => m.DivisibleBy).Aggregate((a,b) => a * b);
        for (var n = 0; n < 10000; n++)
        {
            DoRound(monkeys, n => n % divisor);
        }
        var part2 = monkeys.Select(m => (long)m.InspectCount).OrderDescending().Take(2).Aggregate((a, b) => a * b);

        return ($"{part1}", $"{part2}");
    }

    private static void DoRound(List<Monkey> monkeys, Func<BigInteger,BigInteger> worryReducer)
    {
        foreach (var monkey in monkeys)
        {
            for (var i = 0; i < monkey.Items.Count; i++)
            {
                var item = monkey.Items[i];
                item = monkey.Operation(item);
                item = worryReducer(item);
                if (item % monkey.DivisibleBy == 0)
                {
                    monkeys[monkey.TrueTarget].Items.Add(item);
                }
                else
                {
                    monkeys[monkey.FalseTarget].Items.Add(item);
                }
            }
            monkey.InspectCount += monkey.Items.Count;
            monkey.Items.Clear();
        }
    }

    private static List<Monkey> ParseMonkeys(string[] input)
    {
        var regex = new Regex("\\d+");
        var monkeys = new List<Monkey>();
        // parse monkeys
        foreach (var x in input.Chunk(7))
        {
            var m = new Monkey();
            m.Items.AddRange(regex.Matches(x[1]).Select(m => (BigInteger)int.Parse(m.Value)));
            var op = x[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (op[4] == "*")
                m.Operation = op[5] == "old" ? (x => x * x) : (x => x * int.Parse(op[5]));
            else if (op[4] == "+")
                m.Operation = op[5] == "old" ? (x => x + x) : (x => x + int.Parse(op[5]));


            m.DivisibleBy = int.Parse(regex.Match(x[3]).Value);
            m.TrueTarget = int.Parse(regex.Match(x[4]).Value);
            m.FalseTarget = int.Parse(regex.Match(x[5]).Value);

            monkeys.Add(m);
        }

        return monkeys;
    }
}
