using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day22 : ISolver
{
    public (string, string) ExpectedResult => ("75254", "108311");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    private static readonly Coord[] directions = new Coord[] { (1, 0), (0, 1), (-1, 0), (0, -1) };

    enum Direction { Right, Down, Left, Up };
    enum Edge { Right, Bottom, Left, Top };
    enum CubeFace { Front, Back, Up, Down, Left, Right };

    long Part1(string[] input)
    {
        var grid = ParseGrid(input, out var currentPos);
        Console.WriteLine($"starting at {currentPos}");
        // Initially, you are facing to the right
        // directions clockwise go right, down, left, up
        var directionIndex = 0;
        var instructions = Regex.Matches(input.Last(), "[RL]|(\\d+)").Select(m => m.Value);
        var teleports = GenerateTeleports(grid);

        (currentPos, directionIndex) = FollowInstructions(grid, directionIndex, instructions, currentPos, teleports);
        return CalculatePassword(directionIndex, currentPos);
    }

    long Part2(string[] input)
    {
        var grid = ParseGrid(input, out var currentPos);
        Console.WriteLine($"starting at {currentPos}");
        // Initially, you are facing to the right
        // directions clockwise go right, down, left, up
        var directionIndex = 0;
        var instructions = Regex.Matches(input.Last(), "[RL]|(\\d+)").Select(m => m.Value);
        var teleports = new Dictionary<(Coord, Coord), (Coord, Coord)>();
        var blockSize = 50;

        // there are seven unconnected edges in our test cube
        // x U R  (0,0)  (1,0)  (2,0)
        // x F x  (0,1)  (1,1)  (2,1)
        // L D x  (0,2)  (1,2)  (2,2)
        // B x x  (0,3)  (1,3)  (2,3)
        var positions = new Dictionary<CubeFace, (int, int)>()
        {
            { CubeFace.Up, (1,0) }, 
            { CubeFace.Right, (2,0) }, 
            { CubeFace.Front, (1,1) }, 
            { CubeFace.Left, (0,2) }, 
            { CubeFace.Down, (1,2) }, 
            { CubeFace.Back, (0,3) }
        };
        // with flip 51263 too low
        // without flip 132349 (too high)
        var connections = new List<(Edge, CubeFace, Edge, CubeFace)>()
        {
            // left of F to top of L -ok
            (Edge.Left, CubeFace.Front, Edge.Top, CubeFace.Left),

            // right of F to bottom of R - ok
            (Edge.Right, CubeFace.Front, Edge.Bottom, CubeFace.Right),
            
            // right of B to bottom of D - ok
            (Edge.Right, CubeFace.Back, Edge.Bottom, CubeFace.Down),
            
            // top of U to LHS of B - ok
            (Edge.Top, CubeFace.Up, Edge.Left, CubeFace.Back),
            
            // LHS of U to LHS of L (going left becomes going right) (needs a flip?)
            (Edge.Left, CubeFace.Up, Edge.Left, CubeFace.Left),

            // RHS of R to RHS of D (going right becomes going left) (needs a flip?)
            (Edge.Right, CubeFace.Right, Edge.Right, CubeFace.Down),
            
            // top of R to bottom of B - ok
            (Edge.Top, CubeFace.Right, Edge.Bottom, CubeFace.Back),

        };

        foreach (var (edge1, cubeFace1, edge2, cubeFace2) in connections)
        {
            ConnectSides(teleports, blockSize, positions, edge1, cubeFace1, edge2, cubeFace2);
            ConnectSides(teleports, blockSize, positions, edge2, cubeFace2, edge1, cubeFace1);
        }

        (currentPos, directionIndex) = FollowInstructions(grid, directionIndex, instructions, currentPos, teleports);
        return CalculatePassword(directionIndex, currentPos);
    }

    private void ConnectSides(Dictionary<(Coord, Coord), (Coord, Coord)> teleports, int blockSize, Dictionary<CubeFace, (int, int)> positions, Edge edge1, CubeFace cubeFace1, Edge edge2, CubeFace cubeFace2)
    {
        Console.WriteLine($"Connecting {edge1} edge of {cubeFace1} to {edge2} edge of {cubeFace2} (reverse = {edge1 == edge2})");
        var edge1Coords = GetEdgeCoords(blockSize, positions[cubeFace1], edge1);
        var edge2Coords = GetEdgeCoords(blockSize, positions[cubeFace2], edge2);
        if (edge1 == edge2) edge1Coords = edge1Coords.Reverse();

        foreach (var (a, b) in edge1Coords.Zip(edge2Coords, (a, b) => (a, b)))
        {
            var newDirection = edge2 switch
            {
                Edge.Top => Direction.Down,
                Edge.Right => Direction.Left,
                Edge.Bottom => Direction.Up,
                Edge.Left => Direction.Right,
                _ => throw new InvalidOperationException("Unknown edge")
            };
            teleports.Add((a, a + directions[(int)edge1]), (b, directions[(int)newDirection]));
        }
    }

    private IEnumerable<Coord> GetEdgeCoords(int blockSize, (int x, int y) blockPos, Edge edge)
    {
        var topLeft = new Coord(blockSize * blockPos.x, blockSize * blockPos.y);
        if (edge == Edge.Left)
        {
            for(var n = 0; n < blockSize; n++)
            {
                yield return (topLeft.X, topLeft.Y + n);
            }
        }
        else if (edge == Edge.Right)
        {
            for (var n = 0; n < blockSize; n++)
            {
                yield return (topLeft.X + blockSize - 1, topLeft.Y + n);
            }
        }
        else if (edge == Edge.Top)
        {
            for (var n = 0; n < blockSize; n++)
            {
                yield return (topLeft.X + n, topLeft.Y);
            }
        }
        else if (edge == Edge.Bottom)
        {
            for (var n = 0; n < blockSize; n++)
            {
                yield return (topLeft.X + n, topLeft.Y + blockSize - 1);
            }
        }
    }
    private static Grid<char> ParseGrid(string[] input, out Coord startPos)
    {
        var maxWidth = input.Take(input.Length - 1).Max(line => line.Length);
        var gridLines = input.TakeWhile(line => !String.IsNullOrEmpty(line)).Select(line => line.PadRight(maxWidth)).ToArray();
        var grid = Grid<char>.ParseToGrid(gridLines, c => c);
        // You begin the path in the leftmost open tile of the top row of tiles.
        startPos = grid.LineOut((0, 0), (1, 0)).First(c => grid[c] == '.');
        return grid;
    }

    private static (Coord,int) FollowInstructions(Grid<char> grid, int directionIndex, IEnumerable<string> instructions, Coord currentPos, Dictionary<(Coord, Coord), (Coord, Coord)> teleports)
    {
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
                    var newDirectionIndex = directionIndex;
                    if (teleports.ContainsKey((currentPos, nextPos)))
                    {
                        (nextPos, var newDelta) = teleports[(currentPos, nextPos)];
                        newDirectionIndex = Array.IndexOf(directions, newDelta);
                        if (newDirectionIndex == -1) throw new InvalidOperationException("can't find dir");
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
                    directionIndex = newDirectionIndex;
                    //Console.WriteLine($"move to {currentPos}");
                }
            }

        }
        return (currentPos, directionIndex);
    }

    private static int CalculatePassword(int directionIndex, Coord currentPos)
    {
        var row = currentPos.Y + 1;
        var column = currentPos.X + 1;
        var facing = directionIndex;
        var password = 1000 * row + 4 * column + facing;
        return password;
    }

    private static Dictionary<(Coord, Coord), (Coord, Coord)> GenerateTeleports(Grid<char> grid)
    {
        var teleports = new Dictionary<(Coord, Coord), (Coord, Coord)>();

        // do all the step off the right edge first
        for (var y = 0; y < grid.Height; y++)
        {
            var minX = Enumerable.Range(0, grid.Width).First(x => grid[(x, y)] != ' ');
            var maxX = Enumerable.Range(0, grid.Width).Last(x => grid[(x, y)] != ' ');

            // step left off left edge, appear on right, still facing left
            teleports[((minX, y), (minX - 1, y))] = ((maxX, y), (-1, 0));
            // step right off right edge, appear on left, still facing right
            teleports[((maxX, y), (maxX + 1, y))] = ((minX, y), (1, 0));
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

        return teleports;
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
