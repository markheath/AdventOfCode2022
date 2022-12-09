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
    public void Day9Part1TestInput()
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day9();
        var solution = solver.Solve(testInput,2);
        Assert.AreEqual(13, solution);
    }


    [Test]
    public void Day9Part2TestInput()
    {
        var testInput = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20".Split("\r\n");
        var solver = new Day9();
        var solution = solver.Solve(testInput, 10);
        Assert.AreEqual(36, solution);
    }

}