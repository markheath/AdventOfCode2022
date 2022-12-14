using System.Collections.Generic;
using System.Linq;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day14Tests
{
    private const string TestInput = @"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";

    [Test]
    public void Day14TestInput()
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day14();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("24", "93"), solution);
    }

}