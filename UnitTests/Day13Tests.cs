using System.Collections.Generic;
using System.Linq;
using AdventOfCode2022;
using NUnit.Framework;
using static AdventOfCode2022.Day13;

namespace UnitTests;

public class Day13Tests
{
    private const string TestInput = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";

    [Test]
    public void Day13TestInput()
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day13();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("13", "140"), solution);
    }

    [Test]
    public void Day13ComparisonsCorrect()
    {
        var testInput = TestInput.Split("\r\n");
        var orders = testInput.Chunk(3).Select((chunk, index) => new { Index = index + 1, First = PacketItem.Parse(chunk[0]), Second = PacketItem.Parse(chunk[1]) })
            .Select(p => p.First.CompareTo(p.Second) == -1);
        Assert.AreEqual(new[] { true,true, false,true,false,true,false,false }, orders);
    }

    [Test]
    public void Day13ParseBasicList()
    {
        var p = PacketItem.Parse("[1,1,3,10,1]");
        Assert.AreEqual(5, p.List.Count);
        Assert.AreEqual(new List<int>() { 1,1,3,10,1}, p.List.Select(n => n.Number));
    }

    [Test]
    public void Day13ParseNestedList()
    {
        var input = "[[1],[2,3,4]]";
        var p = PacketItem.Parse(input);
        Assert.AreEqual(2, p.List.Count);
        Assert.AreEqual(new List<int>() { 1, }, p.List[0].List.Select(n => n.Number));
        Assert.AreEqual(new List<int>() { 2,3,4 }, p.List[1].List.Select(n => n.Number));
        Assert.AreEqual(input, p.ToString());
    }

    [Test]
    public void Day13ParseEmptyList()
    {
        var p = PacketItem.Parse("[[[]]]");
        Assert.AreEqual(1, p.List.Count);
        var firstChild = p.List[0];
        Assert.AreEqual(1, firstChild.List.Count);
        var secondChild = firstChild.List[0];
        Assert.AreEqual(0, secondChild.List.Count);
    }
}