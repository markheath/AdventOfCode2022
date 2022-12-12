using AdventOfCode2022;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests;

public class FullAnswerTests
{
    public static IEnumerable<TestCaseData> FindTests()
    {
        return Utils.FindAllSolvers()
            .Select(s => new TestCaseData(s.Solver, s.Input).SetName(s.Solver.GetType().Name));
    }

    [TestCaseSource(nameof(FindTests))]
    public void RunAllDays(ISolver solver, string[] input)
    {
        if (input.Length == 0)
        {
            Assert.Ignore($"No input for {solver.GetType().Name}");
            return;
        }
        var solution = solver.Solve(input);
        Assert.AreEqual(solver.ExpectedResult, solution);
    }
}