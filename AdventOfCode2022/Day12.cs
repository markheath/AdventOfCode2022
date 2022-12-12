namespace AdventOfCode2022;

public class Day12 : ISolver
{
    public (string, string) ExpectedResult => ("497", "492");

    public (string, string) Solve(string[] input)
    {
        var grid = ParseToGrid(input);
        var startPos = grid.AllPositions().First(p => grid[p] == 'S');
        var endPos = grid.AllPositions().First(p => grid[p] == 'E');
        grid[startPos] = 'a';
        grid[endPos] = 'z';

        var part1 = FindShortestDistance(grid, startPos, p => p == endPos, (elevation, testElevation) => elevation + 1 >= testElevation);
        var part2 = FindShortestDistance(grid, endPos, p => grid[p] == 'a', (elevation, testElevation) => testElevation + 1 >= elevation);

        return ($"{part1}", $"{part2}");
    }

    public static Grid<char> ParseToGrid(string[] input)
    {
        var grid = new Grid<char>(input[0].Length, input.Length);
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
                grid[(x, y)] = input[y][x];
        }
        return grid;
    }

    long FindShortestDistance(Grid<char> grid, Coord startPos, Func<Coord,bool> endTest, Func<int,int,bool> canMove)
    {
        var distances = new Dictionary<Coord, int>();

        var positions = new Queue<Coord>();
        positions.Enqueue(startPos);
        distances.Add(startPos, 0);
        var directions = new Coord[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        //Console.WriteLine($"START AT {startPos}");

        while (positions.TryDequeue(out var pos))
        {
            var elevation = grid[pos];
            var distance = distances[pos];
            if (endTest(pos))
            {
                return distance;
            }

            foreach(var direction in directions) 
            {
                var testPos = pos + direction;

                if (grid.IsInGrid(testPos) && !distances.ContainsKey(testPos) )
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
        }
        throw new InvalidOperationException("No route to end");
    }

}
