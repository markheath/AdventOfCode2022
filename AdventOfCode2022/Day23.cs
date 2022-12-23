namespace AdventOfCode2022;

public class Day23 : ISolver
{
    public (string, string) ExpectedResult => ("3874", "948"); // 914 too low, 949 too high

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

    public long Part1(string[] input, bool drawGrids = false)
    {
        var elfPositions = ParseElfPositions(input);
        directionIndex = 0; 
        int minX, maxX, minY, maxY;
        for (var r = 1; r <= 10; r++)
        {
            (elfPositions, var _) = ProposeNewPositions(elfPositions);
            directionIndex++;
            directionIndex %= 4;
            if (drawGrids)
            {
                Console.WriteLine($"== End of round {r} ==");
                GetBounds(elfPositions, out minX, out maxX, out minY, out maxY);
                for (var y = minY; y <= maxY; y++)
                {
                    for (var x = minX; x <= maxX; x++)
                    {
                        Console.Write(elfPositions.Contains((x, y)) ? '#' : '.');
                    }
                    Console.WriteLine();
                }
            }
        }
        GetBounds(elfPositions, out minX, out maxX, out minY, out maxY);
        var area = (maxX - minX + 1) * (maxY - minY + 1);
        return area - elfPositions.Count;
    }

    private static void GetBounds(HashSet<Coord> elfPositions, out int minX, out int maxX, out int minY, out int maxY)
    {
        minX = elfPositions.Min(p => p.X);
        maxX = elfPositions.Max(p => p.X);
        minY = elfPositions.Min(p => p.Y);
        maxY = elfPositions.Max(p => p.Y);
    }

    public long Part2(string[] input)
    {
        var elfPositions = ParseElfPositions(input);
        directionIndex = 0; // don't forget to reset for part 2
        var round = 0;
        var moved = 0;
        do
        {
            (elfPositions, moved) = ProposeNewPositions(elfPositions);
            directionIndex++;
            directionIndex %= 4;
            round++;
        } while (moved > 0);

        return round;
    }

    private (HashSet<Coord>, int) ProposeNewPositions(HashSet<Coord> currentPositions)
    {
        var moved = 0;
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
                    moved--;
                    moves.Remove(newPos);
                    moves.Add(origPos, origPos); // that elf is staying put instead of the original planned move
                }
                newPos = elf; // we're going to stay where we were
            }
            blocked.Add(newPos); // no one else can go here
            if (newPos != elf) moved++;
            moves.Add(newPos, elf);
        }
        if (moves.Count != currentPositions.Count) throw new InvalidOperationException("lost an elf!");
        // moved = moves.Count(kvp => kvp.Key != kvp.Value); // safer count - but gives same result
        return (moves.Keys.ToHashSet(), moved);
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

}
