using System.Text.RegularExpressions;
using AdventOfCode2022;
using SuperLinq;

namespace AdventOfCode2022;

public class Day14 : ISolver
{
    public (string, string) ExpectedResult => ("828", "");

    public (string, string) Solve(string[] input)
    {
        var (g,offset) = ParseGrid(input);
        Console.WriteLine(g.ToString());
        var part1 = Part1(g, offset);
        Console.WriteLine(g.ToString());

        return ($"{part1}", $"{Part2(input)}");
    }

    private (Grid<char>,Coord) ParseGrid(string[] input)
    {
        var lines = input.Select(
               l => Regex.Matches(l, @"\d+")
                        .Select(m => int.Parse(m.Value))
                        .Chunk(2)
                        .Select(c => new Coord(c[0], c[1]))
                        .ToList())
                .ToList();
        var lowestY = lines.Max(l => l.Max(p => p.Y));
        var minX = lines.Min(l => l.Min(p => p.X));
        var maxX = lines.Max(l => l.Max(p => p.X));
        var offset = new Coord(1 - minX, 0);
        //Console.WriteLine($"lowest y {lowestY}  min x {minX} max x {maxX}");
        // add 2 for space for sand to fall down past the bottom
        var grid = new Grid<char>(maxX - minX + 1 + 2, lowestY + 1, '.');
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
        return (grid,offset);
    }

    long Part1(Grid<char> grid, Coord offset)
    {
        var grain = 0;
        while(true)
        {
            var sandPos = new Coord(500, 0) + offset;
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
            if (sandPos.Y == grid.Height - 1)
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
    long Part2(IEnumerable<string> input) => 0;

}
