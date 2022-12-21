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
        public bool Solved { get; private set; }
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
        public IEnumerable<Calc> Rearrange()
        {
            var a = Result;
            var b = LeftHand;
            var c = RightHand;
            var op = Operator;
            if (op == "+")
            {
                // a = b + c so b = a - c and c = a - b
                yield return new Calc($"{b}: {a} - {c}");
                yield return new Calc($"{c}: {a} - {b}");
            }
            else if (op == "-")
            {
                // a = b - c so b = a + c and c = b - a
                yield return new Calc($"{b}: {a} + {c}");
                yield return new Calc($"{c}: {b} - {a}");
            }
            else if (op == "*")
            {
                // a = b * c so b = a / c and c = a / b
                yield return new Calc($"{b}: {a} / {c}");
                yield return new Calc($"{c}: {a} / {b}");
            }
            else if (op == "/")
            {
                // a = b / c so b = a * c and c = b / a
                yield return new Calc($"{b}: {a} * {c}");
                yield return new Calc($"{c}: {b} / {a}");
            }
        }
        public bool Solve(IDictionary<string, long> answers, bool recurse = true)
        {
            if (answers.ContainsKey(LeftHand) && answers.ContainsKey(RightHand))
            {
                answers[Result] = Operator switch
                {
                    "+" => answers[LeftHand] + answers[RightHand],
                    "-" => answers[LeftHand] - answers[RightHand],
                    "/" => answers[LeftHand] / answers[RightHand],
                    "*" => answers[LeftHand] * answers[RightHand],
                    _ => throw new InvalidOperationException($"Unknown operator [{Operator}]")
                };
                Solved = true;
                return true;
            }
            if (recurse)
            {
                foreach (var rearranged in Rearrange())
                {
                    if (rearranged.Solve(answers, false))
                    {
                        Solved = true;
                        return true;
                    }
                }
            }
            return false;
        }
    }

    long Part1(IEnumerable<string> input)
    {
        ParseInput(input, out var calcs, out var answers);
        return Seek("root", calcs, answers, false);
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

        return Seek("humn", calcs, answers, true);

    }

    private static long Seek(string target, List<Calc> calcs, Dictionary<string, long> answers, bool rearrange)
    {
        while (!answers.ContainsKey(target))
        {
            bool solved = false;
            foreach (var calc in calcs.Where(c => !c.Solved))
            {
                solved |= calc.Solve(answers, rearrange);
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
