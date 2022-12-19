using System;
using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day19Tests
{
    private const string TestInput = @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay.  Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";


    [Test]
    public void Day19Part1TestInput()
    {
        var solver = new Day19();
        var blueprints = TestInput.Split("\r\n");
        // blueprint 1 - 9 in 24
        var blueprint1 =solver.Part1(blueprints[0], 24);
        Assert.AreEqual(9, blueprint1);
        var blueprint2 = solver.Part1(blueprints[1], 24);
        Assert.AreEqual(12, blueprint2);
    }

}