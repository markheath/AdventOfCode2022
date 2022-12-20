using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day20Tests
{
    private const string TestInput = @"1
2
-3
3
-2
0
4";

    [Test]
    public void Day20Part1TestInput()
    {
        var solver = new Day20();
        var n = solver.Part1(TestInput.Split("\r\n"));
        Assert.AreEqual(3, n);
        
    }


    [Test]
    public void Day20Part2TestInput()
    {
        var solver = new Day20();
        var n = solver.Part2(TestInput.Split("\r\n"));
        Assert.AreEqual(1623178306, n);
    }

    /*
     * Initial arrangement:
1, 2, -3, 3, -2, 0, 4

1 moves between 2 and -3:
2, 1, -3, 3, -2, 0, 4

2 moves between -3 and 3:
1, -3, 2, 3, -2, 0, 4

-3 moves between -2 and 0:
1, 2, 3, -2, -3, 0, 4

3 moves between 0 and 4:
1, 2, -2, -3, 0, 3, 4

-2 moves between 4 and 1:
1, 2, -3, 0, 3, 4, -2

0 does not move:
1, 2, -3, 0, 3, 4, -2

4 moves between -3 and 0:
1, 2, -3, 4, 0, 3, -2
     */
}