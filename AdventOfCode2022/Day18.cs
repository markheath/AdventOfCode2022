namespace AdventOfCode2022;

public class Day18 : ISolver
{
    public (string, string) ExpectedResult => ("4482", "2576");

    public (string, string) Solve(string[] input)
    {
        var lava = input.Select(line => line.Split(',').Select(int.Parse).ToArray()).Select(c => new Coord3(c[0], c[1], c[2])).ToHashSet();
        var part1 = lava.Sum(c => 6 - c.Neighbours().Count(n => lava.Contains(n)));

        var bottomCorner = new Coord3(lava.Min(p => p.X) - 1, lava.Min(p => p.Y) - 1, lava.Min(p => p.Z) - 1);
        var topCorner = new Coord3(lava.Max(p => p.X) + 1, lava.Max(p => p.Y) + 1, lava.Max(p => p.Z) + 1);
        Console.WriteLine($"{bottomCorner} {topCorner}");
        var outside = 0;
        var queue = new Queue<Coord3>();
        queue.Enqueue(bottomCorner);
        var water = new HashSet<Coord3>() { bottomCorner };
        while(queue.Count > 0)
        {
            var point = queue.Dequeue();
            foreach(var n in point.Neighbours()
                .Where(x => x >= bottomCorner && x <= topCorner
                && !water.Contains(x)))
            {
                if (lava.Contains(n))
                {
                    outside++;
                }
                else
                {
                    water.Add(n);
                    queue.Enqueue(n);
                }
            }
        }


        return ($"{part1}", $"{outside}");
    }

    long Part2(IEnumerable<string> input) => throw new NotImplementedException();

}
