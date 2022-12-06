using System.Collections.Generic;
using System.Diagnostics.Metrics;
using AdventOfCode2022;
using NUnit.Framework;
namespace UnitTests;

public class Day6Tests
{
    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz",5)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",10)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]

    public void Part1TestCases(string input, int expectedPosition)
    {
        var solver = new Day6();
        var solution = solver.FindMarkerPosition(input);
        Assert.AreEqual(expectedPosition, solution);
    }
}