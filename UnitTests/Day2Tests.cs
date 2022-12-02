using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day2Tests
{
    [Test]
    public void SolveWithTestInput()
    {
        var testInput = @"A Y
B X
C Z".Split("\r\n");
        var solver = new Day2();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("15", "12"), solution);
    }
}