using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day22 : ISolver
{
    public (string, string) ExpectedResult => ("75254", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    long Part1(string[] input)
    {
        var maxWidth = input.Take(input.Length-1).Max(line => line.Length);
        var gridLines = input.TakeWhile(line => !String.IsNullOrEmpty(line)).Select(line => line.PadRight(maxWidth)).ToArray();
        var grid = Grid<char>.ParseToGrid(gridLines, c => c);
        // You begin the path in the leftmost open tile of the top row of tiles.
        var startPos = grid.LineOut((0, 0), (1, 0)).First(c => grid[c] == '.');
        Console.WriteLine($"starting at {startPos}");
        // Initially, you are facing to the right
        // directions clockwise go right, down, left, up
        var directions = new Coord[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
        var directionIndex = 0;
        var instructions = Regex.Matches(input.Last(), "[RL]|(\\d+)").Select(m => m.Value);
        var currentPos = startPos;
        var teleports = new Dictionary<(Coord, Coord), (Coord, Coord)>();
        
        // do all the step off the right edge first
        for(var y = 0; y < grid.Height; y++)
        {
            var minX = Enumerable.Range(0, grid.Width).First(x => grid[(x, y)] != ' ');
            var maxX = Enumerable.Range(0, grid.Width).Last(x => grid[(x, y)] != ' ');

            // step left off left edge, appear on right, still facing left
            teleports[((minX, y), (minX - 1, y))] = ((maxX, y), (-1, 0));
            // step right off right edge, appear on left, still facing right
            teleports[((maxX, y), (maxX + 1,y))] = ((minX, y), (1, 0));
        }
        for (var x = 0; x < grid.Width; x++)
        {
            var minY = Enumerable.Range(0, grid.Height).First(y => grid[(x, y)] != ' ');
            var maxY = Enumerable.Range(0, grid.Height).Last(y => grid[(x, y)] != ' ');

            // step up off top edge, appear at bottom, still facing up
            teleports[((x, minY), (x, minY - 1))] = ((x, maxY), (0, -1));
            // step down off bottom edge, appear at top, still facing down
            teleports[((x, maxY), (x, maxY + 1))] = ((x, minY), (0, 1));
        }


        foreach (var instruction in instructions)
        {
            if (instruction == "R")
            {
                directionIndex++;
                directionIndex %= directions.Length;
            }
            else if (instruction == "L")
            {
                directionIndex--;
                if (directionIndex < 0) directionIndex += directions.Length;
            }
            else
            {
                for (var step = 0; step < int.Parse(instruction); step++)
                {
                    var delta = directions[directionIndex];
                    var nextPos = currentPos + delta;
                    if (teleports.ContainsKey((currentPos,nextPos)))
                    {
                        (nextPos, var newDelta) = teleports[(currentPos, nextPos)];
                        directionIndex = Array.IndexOf(directions, newDelta);
                        if (directionIndex == -1) throw new InvalidOperationException("can't find dir");
                    }
                    if (grid[nextPos] == ' ')
                    {
                        // we missed a teleport
                        throw new InvalidOperationException($"oops {nextPos} (was at {currentPos} + {delta}");
                    }

                    //nextPos = WrapRoundIfNeeded(grid, nextPos);
                    //nextPos = MoveThroughEmptySpace(grid, delta, nextPos);
                    if (grid[nextPos] == '#')
                    {
                        // we're blocked by a wall, just stop
                        break;
                    }
                    currentPos = nextPos;
                    //Console.WriteLine($"move to {currentPos}");
                }
            }

        }
        var row = currentPos.Y + 1;
        var column = currentPos.X + 1;
        var facing = directionIndex;
        var password = 1000 * row + 4 * column + facing;
        return password;

    }

    private static Coord MoveThroughEmptySpace(Grid<char> grid, Coord delta, Coord nextPos)
    {
        while (grid.IsInGrid(nextPos) && grid[nextPos] == ' ')
        {
            nextPos += delta;
        }
        nextPos = WrapRoundIfNeeded(grid, nextPos);
        // can still be in empty space after wrapping round
        while (grid.IsInGrid(nextPos) && grid[nextPos] == ' ')
        {
            nextPos += delta;
        }
        return nextPos;
    }

    private static Coord WrapRoundIfNeeded(Grid<char> grid, Coord nextPos)
    {
        if (!grid.IsInGrid(nextPos))
        {
            // fully out of bounds, wrap around (but potentially into empty space)
            nextPos = ((nextPos.X + grid.Width) % grid.Width, (nextPos.Y + grid.Height) % grid.Height);
        }

        return nextPos;
    }

    long Part2(IEnumerable<string> input) => 0;

}
