using System.Linq;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day12Tests
{

    [Test]
    public void Day12Part1TestInput()
    {
        var testInput = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi".Split("\r\n");
        var solver = new Day12();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("31", ""), solution);
    }

    [Test]
    public void Day132Part2TestInput()
    {
        var testInput = @"".Split("\r\n");
        var solver = new Day12();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(@"", solution);
    }

}