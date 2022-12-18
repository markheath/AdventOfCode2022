using System;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day18Tests
{
    private const string TestInput = @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5";


    [Test]
    public void Day18Part1TestInput()
    {
        var solver = new Day18();
        var (part1,part2) = solver.Solve(TestInput.Split("\r\n"));
        Assert.AreEqual("64", part1);
    }

}