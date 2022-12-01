using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day1Tests
{
    [Test]
    public void SolveWithTestInput()
    {
        var testInput = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000".Split("\r\n");
        var solver = new Day1();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("24000", "45000"), solution);
    }
}