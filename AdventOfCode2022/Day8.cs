﻿using SuperLinq;

namespace AdventOfCode2022;

public class Day8 : ISolver
{
    public (string, string) ExpectedResult => ("1832", "157320");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public long Part1(string[] input)
    {
        var g = Grid<int>.ParseToGrid(input);
        var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        return g.AllPositions().Count(p =>  dirs.Any(d => g.LineOut(p,d).All(n => g[p] > g[n])));
    }

    public long ScenicScore(Grid<int> g, Coord p)
    {
        var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        return dirs.Select(d => g.LineOut(p, d)
                             .TakeUntil(n => g[p] <= g[n]).Count())
               .Aggregate((a, b) => a * b);
    }

    public long Part2(string[] input)
    {
        var g = Grid<int>.ParseToGrid(input);
        return g.AllPositions().Max(p => ScenicScore(g, p));
        /*
            var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            return g.AllPositions()
               .Max(p => dirs.Select(d => g.LineOut(p, d)
                             .TakeWhile(n => g[p] >= g[n]).Count())

               .Aggregate((a,b) => a*b));
        */
    }

}
