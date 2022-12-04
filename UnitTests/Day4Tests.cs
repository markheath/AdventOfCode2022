using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day4Tests
{
    [Test]
    public void SolveWithTestInput()
    {
        var testInput = @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8".Split("\r\n");
        var solver = new Day4();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("2", ""), solution);
    }
}