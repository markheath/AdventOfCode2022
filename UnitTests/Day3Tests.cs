using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day3Tests
{
    [Test]
    public void SolveWithTestInput()
    {
        var testInput = @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw".Split("\r\n");
        var solver = new Day3();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("157", "70"), solution);
    }
}