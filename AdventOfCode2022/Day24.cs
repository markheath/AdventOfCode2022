namespace AdventOfCode2022;

public class Day24 : ISolver
{
    public (string, string) ExpectedResult => ("292", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    // idea - for every x,y have an equation that says when they are blocked
    // idea - calc new grid is expensive but cache it

    long Part1(string[] input)
    {
        var h = ParseHurricanes(input);
        CacheHurricanes(0, h);
        var startX = input[0].IndexOf('.') - 1;
        var exitX = input.Last().IndexOf('.') - 1;
        var startPos = (startX, -1);
        var targetPos = (exitX, input.Length - 2);
        var innerWidth = input[0].Length - 2;
        int innerHeight = input.Length - 2;
        var bestSoFar = 400; //innerWidth * innerHeight; // set a sensible upper bound
        Console.WriteLine($"upper bound {bestSoFar}");
        Seek(startPos, targetPos, ref bestSoFar, 0, h, innerWidth, innerHeight);
        return bestSoFar;
    }

    void CacheHurricanes(int t, IEnumerable<Hurricane> hurricanes)
    {
        var h = new List<Hurricane>(hurricanes);
        hurricaneCache[t] = (hurricanes.Select(h => h.pos).ToHashSet(), h);
    }

    private Dictionary<int, (HashSet<Coord>, IEnumerable<Hurricane>)> hurricaneCache = new();
    private HashSet<(int, Coord)> seen = new();

    private void Seek(Coord currentPos, Coord targetPos, ref int bestSoFar, int t, 
        IEnumerable<Hurricane> hurricanesAtT, int innerWidth, int innerHeight)
    {
        t++;
        if (seen.Contains((t, currentPos))) return;
        seen.Add((t, currentPos));
        if(t > bestSoFar)
        {
            // abandon this route
            return;
        }
        if (!hurricaneCache.ContainsKey(t)) CacheHurricanes(t, MoveHuricanes(hurricanesAtT, innerWidth, innerHeight));
        // to try first right, then down, then stay put, then left
        var deltas = new Coord[] { (1, 0), (0, 1), (-1,0), (0,-1), (0, 0)};
        var blockedByHurricanes = hurricaneCache[t].Item1;
        foreach(var move in deltas)
        {
            var newPos = currentPos + move;
            if (newPos == targetPos)
            {
                if (t < bestSoFar)
                {
                    Console.WriteLine($"new best time {t}");
                    bestSoFar = t;
                }
                return;
            }
            if (blockedByHurricanes.Contains(newPos) || newPos.X < 0 || newPos.Y < 0 || newPos.X >= innerWidth || newPos.Y >= innerHeight)
            {
                continue;
            }
            Seek(newPos,targetPos,ref bestSoFar, t, hurricaneCache[t].Item2, innerWidth, innerHeight);
        }
    }


    // normalize coords so 0,0 is at start of main grid
    public IEnumerable<Hurricane> ParseHurricanes(string[] input)
    {
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                var c = input[y][x];
                if (c == '<' || c == '>' || c == 'v' || c == '^')
                    yield return new Hurricane(c, (x - 1, y - 1));
            }
        }
    }

    public IEnumerable<Hurricane> MoveHuricanes(IEnumerable<Hurricane> hurricanes, int width, int height)
    {
        foreach(var h in hurricanes)
        {
            Coord delta = h.dir switch
            {
                '>' => (1, 0),
                '<' => (-1, 0),
                '^' => (0, -1),
                'v' => (0, 1),
                _ => throw new InvalidOperationException("Unknown hurricane direction")
            };
            var (x,y) = h.pos + delta;
            if (x < 0) x = width - 1;
            if (x >= width) x = 0;
            if (y < 0) y = height - 1;
            if (y >= height) y = 0;
            yield return new Hurricane(h.dir, (x, y));
        }
    }


    public record Hurricane(char dir, Coord pos);

    
    long Part2(IEnumerable<string> input) => 0;

}
