namespace AdventOfCode2022;

public class Day21 : ISolver
{
    public (string, string) ExpectedResult => ("276156919469632", "3441198826073");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    long Part1(IEnumerable<string> input)
    {
        var calcs = input.Select(i => new { id = i[0..4], val = i[6..] })
               .Where(m => m.val.Length == 11).ToDictionary(m => m.id, m => m.val.Split(' '));
        var answers = input.Select(i => new { id = i[0..4], val = i[6..] })
               .Where(m => m.val.Length < 11).ToDictionary(m => m.id, m => long.Parse(m.val));
        while(!answers.ContainsKey("root"))
        {
            bool solved = false;
            foreach(var calc in calcs.Where(c => !answers.ContainsKey(c.Key) && answers.ContainsKey(c.Value[0]) && answers.ContainsKey(c.Value[2])))
            {

                answers[calc.Key] = calc.Value[1] switch
                {
                    "+" => answers[calc.Value[0]] + answers[calc.Value[2]],
                    "-" => answers[calc.Value[0]] - answers[calc.Value[2]],
                    "/" => answers[calc.Value[0]] / answers[calc.Value[2]],
                    "*" => answers[calc.Value[0]] * answers[calc.Value[2]],
                    _ => throw new InvalidOperationException("Unknown operator")
                };
                solved = true;
            }
            if (!solved)
                throw new InvalidOperationException("failed to solve any calc this round");
        }
        return answers["root"];
    }
    long Part2(IEnumerable<string> input)
    {
        var calcs = input.Select(i => new { id = i[0..4], val = i[6..] })
               .Where(m => m.val.Length == 11)
               .Select(m => KeyValuePair.Create(m.id, m.val.Split(' ')))
               .ToList();
        var answers = input.Select(i => new { id = i[0..4], val = i[6..] })
               .Where(m => m.val.Length < 11).ToDictionary(m => m.id, m => long.Parse(m.val));
        
        // take out the human answer - it's what we're solving for
        var r = answers.Remove("humn");
        if (!r) throw new InvalidOperationException("oops");

        // replace the old root = a + b calculation with saying that root = a - b and root = 0
        answers["root"] = 0;
        var rootCalc = calcs.Single(c => c.Key == "root");
        calcs.Remove(rootCalc);
        calcs.Add(KeyValuePair.Create("root", new[] { rootCalc.Value[0], "-", rootCalc.Value[2] }));

        // put in all variations of all calcs
        foreach(var calc in calcs.ToList())
        {
            var a = calc.Key;
            var b = calc.Value[0];
            var c = calc.Value[2];
            var op = calc.Value[1];
            if (op == "+")
            {
                // a = b + c so b = a - c and c = a - b
                calcs.Add(KeyValuePair.Create(b, new[] { a, "-", c }));
                calcs.Add(KeyValuePair.Create(c, new[] { a, "-", b }));
            }
            else if (op == "-")
            {
                // a = b - c so b = a + c and c = b - a
                calcs.Add(KeyValuePair.Create(b, new[] { a, "+", c }));
                calcs.Add(KeyValuePair.Create(c, new[] { b, "-", a }));
            }
            else if (op == "*")
            {
                // a = b * c so b = a / c and c = a / b
                calcs.Add(KeyValuePair.Create(b, new[] { a, "/", c }));
                calcs.Add(KeyValuePair.Create(c, new[] { a, "/", b }));
            }
            else if (op == "/")
            {
                // a = b / c so b = a * c and c = b / a
                calcs.Add(KeyValuePair.Create(b, new[] { a, "*", c }));
                calcs.Add(KeyValuePair.Create(c, new[] { b, "/", a }));
            }
        }


        while (!answers.ContainsKey("humn"))
        {
            bool solved = false;
            foreach (var calc in calcs.Where(c => !answers.ContainsKey(c.Key) && answers.ContainsKey(c.Value[0]) && answers.ContainsKey(c.Value[2])))
            {

                answers[calc.Key] = calc.Value[1] switch
                {
                    "+" => answers[calc.Value[0]] + answers[calc.Value[2]],
                    "-" => answers[calc.Value[0]] - answers[calc.Value[2]],
                    "/" => answers[calc.Value[0]] / answers[calc.Value[2]],
                    "*" => answers[calc.Value[0]] * answers[calc.Value[2]],
                    _ => throw new InvalidOperationException("Unknown operator")
                };
                solved = true;
            }
            if (!solved)
                throw new InvalidOperationException("failed to solve any calc this round");
        }
        return answers["humn"];

    }

}
