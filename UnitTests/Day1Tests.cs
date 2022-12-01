using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day1Tests
{
    [Test]
    public void SolveWithTestInput()
    {
        var testInput = @"".Split("\r\n");
        var solver = new Day1();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("", ""), solution);
    }
}