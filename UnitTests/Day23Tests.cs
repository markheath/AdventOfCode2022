using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day23Tests
{
    private const string TestInput = @"..............
..............
.......#......
.....###.#....
...#...#.#....
....#...##....
...#.###......
...##.#.##....
....#..#......
..............
..............
..............";

    [Test]
    public void Day23TestInput()
    {
        var solver = new Day23();
        var n = solver.Solve(TestInput.Split("\r\n"));
        Assert.AreEqual(("110", "20"), n);       
    }

    [Test]
    public void Day23ShowGrids()
    {
        var solver = new Day23();
        var n = solver.Part1(TestInput.Split("\r\n"), true);
        Assert.AreEqual(110, n);
    }

}