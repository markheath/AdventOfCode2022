using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day21 : ISolver
{
    public (string, string) ExpectedResult => ("276156919469632", "3441198826073");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    class Calc
    {
        public string Result { get; }
        public string LeftHand { get; }
        public string RightHand { get; }
        public string Operator { get; }
        public Calc(string s)
        {
            var m = Regex.Matches(s, "[a-z]+");
            Result = m[0].Value;
            LeftHand = m[1].Value;
            Operator = Regex.Match(s, @"[\*\+\-\/]").Value;
            RightHand = m[2].Value;
        }
        public override string ToString()
        {
            return $"{Result}={LeftHand}{Operator}{RightHand}";
        }
    }

    long Part1(IEnumerable<string> input)
    {
        ParseInput(input, out var calcs, out var answers);
        return Seek("root", calcs, answers);
    }

    long Part2(IEnumerable<string> input)
    {
        ParseInput(input, out var calcs, out var answers);

        // take out the human answer - it's what we're solving for
        var r = answers.Remove("humn");
        if (!r) throw new InvalidOperationException("oops");

        // replace the old root = a + b calculation with saying that root = a - b and root = 0
        answers["root"] = 0;
        var rootCalc = calcs.Single(c => c.Result == "root");
        calcs.Remove(rootCalc);
        calcs.Add(new Calc($"root: {rootCalc.LeftHand} - {rootCalc.RightHand}"));

        // put in all variations of all calcs
        foreach (var calc in calcs.ToList())
        {
            var a = calc.Result;
            var b = calc.LeftHand;
            var c = calc.RightHand;
            var op = calc.Operator;
            if (op == "+")
            {
                // a = b + c so b = a - c and c = a - b
                calcs.Add(new Calc($"{b}: {a} - {c}"));
                calcs.Add(new Calc($"{c}: {a} - {b}"));
            }
            else if (op == "-")
            {
                // a = b - c so b = a + c and c = b - a
                calcs.Add(new Calc($"{b}: {a} + {c}"));
                calcs.Add(new Calc($"{c}: {b} - {a}"));
            }
            else if (op == "*")
            {
                // a = b * c so b = a / c and c = a / b
                calcs.Add(new Calc($"{b}: {a} / {c}"));
                calcs.Add(new Calc($"{c}: {a} / {b}"));
            }
            else if (op == "/")
            {
                // a = b / c so b = a * c and c = b / a
                calcs.Add(new Calc($"{b}: {a} * {c}"));
                calcs.Add(new Calc($"{c}: {b} / {a}"));
            }
        }

        return Seek("humn", calcs, answers);

    }

    private static long Seek(string target, List<Calc> calcs, Dictionary<string, long> answers)
    {
        while (!answers.ContainsKey(target))
        {
            bool solved = false;
            foreach (var calc in calcs.Where(c => !answers.ContainsKey(c.Result) && answers.ContainsKey(c.LeftHand) && answers.ContainsKey(c.RightHand)))
            {
                // Console.WriteLine($"SOLVING {calc}");
                answers[calc.Result] = calc.Operator switch
                {
                    "+" => answers[calc.LeftHand] + answers[calc.RightHand],
                    "-" => answers[calc.LeftHand] - answers[calc.RightHand],
                    "/" => answers[calc.LeftHand] / answers[calc.RightHand],
                    "*" => answers[calc.LeftHand] * answers[calc.RightHand],
                    _ => throw new InvalidOperationException($"Unknown operator [{calc.Operator}]")
                };
                solved = true;
            }
            if (!solved)
                throw new InvalidOperationException("failed to solve any calc this round");
        }
        return answers[target];
    }

    private static void ParseInput(IEnumerable<string> input, out List<Calc> calcs, out Dictionary<string, long> answers)
    {
        calcs = input
               .Where(m => m.Length > 13)
               .Select(m => new Calc(m))
               .ToList();
        answers = input.Select(i => new { id = i[0..4], val = i[6..] })
               .Where(m => m.val.Length < 11).ToDictionary(m => m.id, m => long.Parse(m.val));
    }
}
