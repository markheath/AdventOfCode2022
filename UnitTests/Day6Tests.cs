using System.Collections.Generic;
using System.Diagnostics.Metrics;
using AdventOfCode2022;
using NUnit.Framework;
namespace UnitTests;

public class Day6Tests
{
    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7, 4)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz",5, 4)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6, 4)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",10, 4)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11, 4)]
    [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19, 14)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 23, 14)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 23, 14)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29, 14)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26, 14)]
    public void Day6TestCases(string input, int expectedPosition, int distinct)
    {
        var solver = new Day6();
        var solution = solver.FindMarkerPosition(input, distinct);
        Assert.AreEqual(expectedPosition, solution);
    }
}