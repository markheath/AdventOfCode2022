using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day12Tests
{

    [Test]
    public void Day12TestInput()
    {
        var testInput = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi".Split("\r\n");
        var solver = new Day12();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("31", "29"), solution);
    }
}