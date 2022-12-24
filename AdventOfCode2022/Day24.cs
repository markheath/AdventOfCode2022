namespace AdventOfCode2022;

public class Day24 : ISolver
{
    public (string, string) ExpectedResult => ("292", "816");

    public (string, string) Solve(string[] input)
    {
        var h = ParseHurricanes(input);
        CacheHurricanes(0, h);
        var startX = input[0].IndexOf('.') - 1;
        var exitX = input.Last().IndexOf('.') - 1;

        entrance = (startX, -1);
        exit = (exitX, input.Length - 2);
        innerWidth = input[0].Length - 2;
        innerHeight = input.Length - 2;
        
        var part1 = Seek(entrance, exit, 0);
        Console.WriteLine($"reached goal at {part1}");
        var part2a = Seek(exit, entrance, part1);
        Console.WriteLine($"got back to start in {part2a}");
        var part2b = Seek(entrance, exit, part1 + part2a);
        Console.WriteLine($"returned to target in {part2b}");
        var part2 = part1 + part2a + part2b;
        return ($"{part1}", $"{part2}");
    }

    int Seek(Coord startPos, Coord targetPos, int startTime)
    {
        var upperLimit = 400; // pick a max depth to start with
        var bestSoFar = startTime + upperLimit; //innerWidth * innerHeight; // set a sensible upper bound
        seen.Clear();
        Seek(startPos, targetPos, ref bestSoFar, startTime);
        var extraTime = bestSoFar - startTime;
        if (extraTime >=  upperLimit) throw new InvalidOperationException($"Failed to find a route shorter than {upperLimit}");
        return extraTime;
    }

    void CacheHurricanes(int t, IEnumerable<Hurricane> hurricanes)
    {
        var h = new List<Hurricane>(hurricanes);
        hurricaneCache[t] = (hurricanes.Select(h => h.pos).ToHashSet(), h);
    }

    private Coord entrance;
    private Coord exit;
    private int innerWidth;
    private int innerHeight;
    private Dictionary<int, (HashSet<Coord>, IEnumerable<Hurricane>)> hurricaneCache = new();
    private HashSet<(int, Coord)> seen = new();

    private void Seek(Coord currentPos, Coord targetPos, ref int bestSoFar, int t)
    {
        t++;
        if (seen.Contains((t, currentPos))) return;
        seen.Add((t, currentPos));
        if(t > bestSoFar)
        {
            // abandon this route
            return;
        }
        if (!hurricaneCache.ContainsKey(t)) CacheHurricanes(t, MoveHuricanes(hurricaneCache[t-1].Item2));
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
                    //Console.WriteLine($"new best time {t}");
                    bestSoFar = t;
                }
                return;
            }
            var isBlocked = (blockedByHurricanes.Contains(newPos) || newPos.X < 0 || newPos.Y < 0 || newPos.X >= innerWidth || newPos.Y >= innerHeight);
            // special case - entrance and exit squares are always free
            if (isBlocked && (newPos == entrance || newPos == exit)) isBlocked = false;
            if (!isBlocked)
                Seek(newPos, targetPos, ref bestSoFar, t);
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

    public IEnumerable<Hurricane> MoveHuricanes(IEnumerable<Hurricane> hurricanes)
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
            if (x < 0) x = innerWidth - 1;
            if (x >= innerWidth) x = 0;
            if (y < 0) y = innerHeight - 1;
            if (y >= innerHeight) y = 0;
            yield return new Hurricane(h.dir, (x, y));
        }
    }


    public record Hurricane(char dir, Coord pos);

}
