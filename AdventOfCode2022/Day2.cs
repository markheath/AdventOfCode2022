namespace AdventOfCode2022;

public class Day2 : ISolver
{
    private readonly Dictionary<char, char> Winners;
    private readonly Dictionary<char, char> Losers;

    public Day2()
    {
        Winners = new Dictionary<char, char> { { 'A', 'C' }, { 'B', 'A' }, { 'C', 'B' } };
        Losers = Winners.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    }
    public (string, string) ExpectedResult => ("11449", "13187");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }
    private const int delta = (int)'X' - 'A';

    long Part1(IEnumerable<string> input) => input.Sum(line => Score(line[0], (char)(line[2] - delta)));
    long Part2(IEnumerable<string> input) => input.Sum(line => Score2(line[0], line[2]));

    /*long Score(int opponent, int myChoice)
    {
        var roundScore = (opponent, myChoice) switch
        {
            ('A', 'C') => 0, // rock beats scissors
            ('B', 'A') => 0, // paper beats rock
            ('C', 'B') => 0, // scissors beats paper
            (int a, int b) when (a == b) => 3, // it's a draw
            _ => 6 // I won
        };
        var choiceScore = myChoice - 'A' + 1;
        return choiceScore + roundScore;
    }*/

    long Score(char opponent, char myChoice)
    {
        var roundScore = opponent == myChoice ? 3 : (myChoice == Losers[opponent]) ? 6 : 0;
        var choiceScore = myChoice - 'A' + 1;
        return choiceScore + roundScore;
    }

    long Score2(char opponent, char strategy)
    {
        var roundScore = strategy switch { 'X' => 0, 'Y' => 3, _ => 6 };
        var myChoice = strategy == 'Y' ? opponent : strategy == 'Z' ? Losers[opponent] : Winners[opponent];
        var choiceScore = myChoice - 'A' + 1;
        return choiceScore + roundScore;
    }


}
