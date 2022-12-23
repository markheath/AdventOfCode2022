using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace AdventOfCode2022;

public class Day23 : ISolver
{
    public (string, string) ExpectedResult => ("3874", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    private int directionIndex; // north, south, west, east

    private static readonly Coord[][] checkSquares = new[] { 
        new Coord[] { (-1,-1), (0,-1), (1,-1) }, // NORTH
        new Coord[] { (-1,1), (0,1), (1,1) }, //  SOUTH
        new Coord[] { (-1,-1), (-1,0), (-1,1) }, // WEST
        new Coord[] { (1,-1), (1,0), (1,1) } // EAST
    };

    public long Part1(string[] input)
    {
        var elfPositions = ParseElfPositions(input);
        for(var r = 0; r < 10; r++)
        {            
            elfPositions = ProposeNewPositions(elfPositions);
            directionIndex++;
            directionIndex %= 4;
        }
        var minX = elfPositions.Min(p => p.X);
        var maxX = elfPositions.Max(p => p.X);
        var minY = elfPositions.Min(p => p.Y);
        var maxY = elfPositions.Max(p => p.Y);
        var area = (maxX - minX + 1) * (maxY - minY + 1);
        return area - elfPositions.Count;
    }

    private HashSet<Coord> ProposeNewPositions(HashSet<Coord> currentPositions)
    {
        HashSet<Coord> blocked = new();
        Dictionary<Coord,Coord> moves = new(); // to, from
        foreach(var elf in currentPositions)
        {
            var newPos = elf; // stay where we are by default
            if (elf.Neighbours(true).All(p => !currentPositions.Contains(p)))
            {
                // we're staying put
            }
            else
            {
                for (var d = 0; d < 4; d++)
                {
                    var checkPositions = checkSquares[(d + directionIndex) % 4];
                    if (checkPositions.All(p => !currentPositions.Contains(p + elf)))
                    {
                        newPos = elf + checkPositions[1];
                        break;
                    }
                }
            }
            if (blocked.Contains(newPos)) // someone else has already tried to go here
            {
                // if another elf was already planning to go here, now they can't
                if (moves.ContainsKey(newPos))
                {
                    var origPos = moves[newPos];
                    moves.Remove(newPos);
                    moves.Add(origPos, origPos); // that elf is staying put instead of the original planned move
                }
                newPos = elf; // we're going to stay where we were
            }
            blocked.Add(newPos); // no one else can go here
            moves.Add(newPos, elf);
        }
        if (moves.Count != currentPositions.Count) throw new InvalidOperationException("lost an elf!");
        return moves.Keys.ToHashSet();
    }

    private static HashSet<Coord> ParseElfPositions(string[] input)
    {
        HashSet<Coord> elfPositions = new();
        for (var y = 0; y < input.Length; y++)
            for (var x = 0; x < input[y].Length; x++)
                if (input[y][x] == '#')
                    elfPositions.Add((x, y));
        return elfPositions;
    }

    long Part2(IEnumerable<string> input) => 0;

}
