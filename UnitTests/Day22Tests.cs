using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day22Tests
{
    private const string TestInput = @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5";

    [Test]
    public void Day22TestInput()
    {
        var solver = new Day22();
        var n = solver.Solve(TestInput.Split("\r\n"));
        Assert.AreEqual(("6032", "0"), n);       
    }
}