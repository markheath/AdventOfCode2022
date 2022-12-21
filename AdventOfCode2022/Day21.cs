namespace AdventOfCode2022;

public class Day21 : ISolver
{
    public (string, string) ExpectedResult => ("276156919469632", "");

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
    long Part2(IEnumerable<string> input) => 0;

}
