using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day11 : ISolver
{
    class Monkey
    {
        public int InspectCount { get; set; }
        public List<long> Items { get; } = new List<long>();
        public Func<long,long> Operation { get; set; }
        public int DivisibleBy { get; set; }
        public int TrueTarget { get; set; }
        public int FalseTarget { get; set; }
    }

    public (string, string) ExpectedResult => ("182293", "");

    public (string, string) Solve(string[] input)
    {
        var regex = new Regex("\\d+");
        var monkeys= new List<Monkey>();
        // parse monkeys
        foreach(var x in input.Chunk(7))
        {
            var m = new Monkey();
            m.Items.AddRange(x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(2).Select(n => long.Parse(n.Trim(','))));
            var op = x[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (op[4] == "*")
                m.Operation = op[5] == "old" ? (x => x * x) : (x => x * int.Parse(op[5]));
            else if (op[4] == "+")
                m.Operation = op[5] == "old" ? (x => x + x) : (x => x + int.Parse(op[5]));


            m.DivisibleBy = int.Parse(regex.Match(x[3]).Value);
            m.TrueTarget = int.Parse(x[4].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last().Trim());
            m.FalseTarget = int.Parse(x[5].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last().Trim());

            monkeys.Add(m);
        }

        // do 20 rounds
        for (var n = 0; n < 20; n++)
        {
            foreach(var monkey in monkeys)
            {
                for(var i = 0; i < monkey.Items.Count; i++)
                {
                    long item = monkey.Items[i];
                    item = monkey.Operation(item);
                    item /= 3;
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
        var part1 = monkeys.Select(m=>(long)m.InspectCount).OrderDescending().Take(2).Aggregate((a, b) => a * b);

        return ($"{part1}", $"0");
    }

    long Part1(IEnumerable<string> input) => throw new NotImplementedException();
    long Part2(IEnumerable<string> input) => throw new NotImplementedException();

}
