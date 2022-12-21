using AdventOfCode2022;
using NUnit.Framework;

namespace UnitTests;

public class Day21Tests
{
    private const string TestInput = @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32";

    [Test]
    public void Day21TestInput()
    {
        var solver = new Day21();
        var n = solver.Solve(TestInput.Split("\r\n"));
        Assert.AreEqual(("152","301"), n);       
    }
}