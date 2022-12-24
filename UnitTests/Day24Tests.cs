using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day24Tests
{
    private const string TestInput = @"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#";

    public void BlockedTimes()
    {
        // 1,1 is never blocked (no blizards cross it)
        // blizard starting at 1,2 goes: 1,2 2,2 3,2 4,2 5,2 1,2 etc
        // x = 1 + (0+t)%5
        // bilzard starting at 4,4 goes 4,4 4,5 4,1 4,2
        // y = 1 + (3+t)%5

    }

    [Test]
    public void Day24TestInput()
    {
        var solver = new Day24();
        var n = solver.Solve(TestInput.Split("\r\n"));
        Assert.AreEqual(("18", "54"), n);       
    }

}