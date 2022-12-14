using System.Text.RegularExpressions;
using AdventOfCode2022;
using SuperLinq;

namespace AdventOfCode2022;

public class Day14 : ISolver
{
    public (string, string) ExpectedResult => ("828", "25500");

    public (string, string) Solve(string[] input)
    {
        var (g,offset) = ParseGrid(input, false);
        //Console.WriteLine(g.ToString());
        var startingPos = new Coord(500, 0) + offset;
        var part1 = SimulateSand(g, startingPos, sandPos => sandPos.Y == g.Height - 1);
        //Console.WriteLine(g.ToString());

        (g, offset) = ParseGrid(input, true);
        startingPos = new Coord(500, 0) + offset;
        Console.WriteLine(g.ToString());
        var part2 = 1 + SimulateSand(g, startingPos, sandPos => sandPos == startingPos);

        return ($"{part1}", $"{part2}");
    }

    private (Grid<char>,Coord) ParseGrid(string[] input, bool part2)
    {
        var lines = input.Select(
               l => Regex.Matches(l, @"\d+")
                        .Select(m => int.Parse(m.Value))
                        .Chunk(2)
                        .Select(c => new Coord(c[0], c[1]))
                        .ToList())
                .ToList();
        var lowestY = lines.Max(l => l.Max(p => p.Y));
        if (part2) lowestY += 2;
        var minX = lines.Min(l => l.Min(p => p.X));
        var maxX = lines.Max(l => l.Max(p => p.X));
        if (part2)
        {
            // enough width for essentially a triangle shape
            minX = Math.Min(minX,500 - lowestY);
            maxX = Math.Max(maxX, 500 + lowestY);
        }
        //Console.WriteLine($"lowest y {lowestY}  min x {minX} max x {maxX}");
        // add 2 for space for sand to fall down past the bottom
        //var grid = new Grid<char>(maxX - minX + 1 + 2, lowestY + 1, '.');
        //var offset = new Coord(1 + grid.Width / 2 - 500, 0);
        var grid = new Grid<char>(1000, lowestY + 1, '.');
        var offset = new Coord(0, 0);

        //grid[sandPos] = 'o';
        foreach (var line in lines)
        {
            foreach (var pair in line.Window(2))
            {
                foreach (var p in pair[0].LineTo(pair[1]))
                {
                    grid[p + offset] = '#';
                }

            }
        }
        if (part2)
        {
            // draw the floor
            for(var x = 0; x < grid.Width; x++)
            {
                grid[(x, grid.Height - 1)] = '#';
            }
        }
        return (grid,offset);
    }

    long SimulateSand(Grid<char> grid, Coord startingPos, Func<Coord, bool> exitCondition)
    {
        var grain = 0;
        while(true)
        {
            var sandPos = startingPos;
            if (grain % 20 == 0) Console.WriteLine($"GRID {grain}\r\n{grid}");
            // fall down until stopped
            while (sandPos.Y < grid.Height - 1)
            {
                if (grid[sandPos + (0, 1)] == '.')
                {
                    sandPos += (0, 1);
                }
                else if (grid[sandPos + (-1, 1)] == '.')
                {
                    sandPos += (-1, 1);
                }
                else if (grid[sandPos + (1, 1)] == '.')
                {
                    sandPos += (1, 1);
                }
                else
                {
                    break;
                }
            }
            if (exitCondition(sandPos))
            {
                break;
            }
            else
            {
                grid[sandPos] = 'O';
                grain += 1;

            }
        }
        return grain;
     }

}
