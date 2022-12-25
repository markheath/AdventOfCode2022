namespace AdventOfCode2022;

public class Day25 : ISolver
{
    public (string, string) ExpectedResult => ("2-=2==00-0==2=022=10", "");

    public (string, string) Solve(string[] input)
    {
        var part1 = DecimalToSnafu(input.Select(SnafuToDecimal).Sum());

        return ($"{part1}", $"");
    }

    public static long SnafuToDecimal(string input)
    {
        var n = 0L;
        var multiplier = 1L;
        var digits = new[] { '=', '-', '0', '1', '2',  };
        foreach (var d in input.Reverse())
        {
            n += (Array.IndexOf(digits, d) - 2) * multiplier;
            multiplier*= 5;
        }
        return n;
    }

    public static string DecimalToSnafu(long number)
    {
        var snafu = "";
        var digits = new[] { '0', '1', '2', '=', '-' };
        while(number > 0)
        {
            var d = digits[number % 5];
            number = (number + 2) / 5;
            snafu += d;
        }
        return new string(snafu.Reverse().ToArray());
    }


}
