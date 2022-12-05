using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day5Tests
{
    [Test]
    public void SolveWithTestInput()
    {
        var testInput = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2".Split("\r\n");
        var solver = new Day5();
        var solution = solver.Solve(testInput);
        Assert.AreEqual(("CMZ", ""), solution);
    }
}