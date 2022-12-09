using System.Linq;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day9Tests
{

    private const string TestInput = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

    [Test]
    public void Day9TestInput()
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day9();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("13", "0"), solution);
    }

}