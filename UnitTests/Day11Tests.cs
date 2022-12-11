using System.Linq;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day11Tests
{

    [Test]
    public void Day11Part1TestInput()
    {
        var testInput = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1".Split("\r\n");
        var solver = new Day11();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("10605", "0"), solution);
    }

    [Test]
    public void Day11Part2TestInput()
    {
        var testInput = @"".Split("\r\n");
        var solver = new Day11();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(@"", solution);
    }

}