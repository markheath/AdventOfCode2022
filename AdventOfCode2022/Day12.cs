namespace AdventOfCode2022;

public class Day12 : ISolver
{
    public (string, string) ExpectedResult => ("497", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
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

    long Part1(string[] input)
    {
        var grid = ParseToGrid(input);
        var startPos = grid.AllPositions().First(p => grid[p] == 'S');
        var endPos = grid.AllPositions().First(p => grid[p] == 'E');
        var distances = new Dictionary<Coord, int>();
        grid[startPos] = 'a';
        grid[endPos] = 'z';
        var positions = new Queue<Coord>();
        positions.Enqueue(startPos);
        distances.Add(startPos, 0);
        var directions = new Coord[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        while(positions.TryDequeue(out var pos))
        {
            var elevation = grid[pos];
            var distance = distances[pos];
            if (pos == endPos)
            {
                return distance;
            }

            foreach(var direction in directions) 
            {
                var testPos = pos + direction;

                if (grid.IsInGrid(testPos) && !distances.ContainsKey(testPos) )
                {
                    var testElevation = grid[testPos];
                    if (elevation + 1 >= testElevation)
                    {
                        Console.WriteLine($"Distance to {testPos} is {distance+1}");
                        distances[testPos] = distance + 1;
                        positions.Enqueue(testPos);
                    }
                }
                else
                {
                    Console.WriteLine($"ignore {testPos}");
                }
            }
        }
        throw new InvalidOperationException("No route to end");
    }
    long Part2(IEnumerable<string> input) => 0;

}
