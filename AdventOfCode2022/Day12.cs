namespace AdventOfCode2022;

public class Day12 : ISolver
{
    public (string, string) ExpectedResult => ("497", "492");

    public (string, string) Solve(string[] input)
    {
        var grid = Grid<char>.ParseToGrid(input, c=> c);
        var startPos = grid.AllPositions().First(p => grid[p] == 'S');
        var endPos = grid.AllPositions().First(p => grid[p] == 'E');
        grid[startPos] = 'a';
        grid[endPos] = 'z';

        var part1 = FindShortestDistance(grid, startPos, p => p == endPos, (elevation, testElevation) => elevation + 1 >= testElevation);
        var part2 = FindShortestDistance(grid, endPos, p => grid[p] == 'a', (elevation, testElevation) => testElevation + 1 >= elevation);

        return ($"{part1}", $"{part2}");
    }

    long FindShortestDistance(Grid<char> grid, Coord startPos, Func<Coord,bool> endTest, Func<int,int,bool> canMove)
    {
        var distances = new Dictionary<Coord, int>() { { startPos, 0 } };
        var positions = new Queue<Coord>(new[] { startPos });
        //Console.WriteLine($"START AT {startPos}");

        while (positions.TryDequeue(out var pos))
        {
            var elevation = grid[pos];
            var distance = distances[pos];
            if (endTest(pos))
            {
                return distance;
            }

            foreach(var testPos in grid.Neighbours(pos).Where(p => !distances.ContainsKey(p)))
            {
                var testElevation = grid[testPos];
                if (canMove(elevation,testElevation))
                {
                    //Console.WriteLine($"Distance to {testPos} is {distance+1}");
                    distances[testPos] = distance + 1;
                    positions.Enqueue(testPos);
                }
            }
        }
        throw new InvalidOperationException("No route to end");
    }

}
