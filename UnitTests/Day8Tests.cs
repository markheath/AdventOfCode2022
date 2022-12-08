using System.Linq;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day8Tests
{

    private const string TestInput = @"30373
25512
65332
33549
35390";

    [Test]
    public void Part1()
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day8();
        var part1 = solver.Part1(testInput);

        Assert.That(part1, Is.EqualTo(21));
    }

}