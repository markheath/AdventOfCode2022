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
        var maxWidth = input.Max(line => line.Length);
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
                    nextPos = WrapRoundIfNeeded(grid, nextPos);
                    nextPos = MoveThroughEmptySpace(grid, delta, nextPos);
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
