namespace AdventOfCode2022;

public class Day9 : ISolver
{
    public (string, string) ExpectedResult => ("6030", "2545");

    public (string, string) Solve(string[] input)
    {
        return ($"{Solve(input,2)}", $"{Solve(input,10)}");
    }

    public long Solve(IEnumerable<string> input, int ropeLength)
    {
        var rope = Enumerable.Range(0, ropeLength).Select(_ => new Coord(0, 0)).ToArray(); 
        var tailVisited = new HashSet<Coord>();
        tailVisited.Add(rope.Last());
        foreach (var inst in input)
        {
            var dir = inst[0] switch { 'R' => (1, 0), 'L' => (-1, 0), 'U' => (0, 1), 'D' => (0, -1) };
            var count = int.Parse(inst[2..]);
            for(var step = 0; step < count; step++)
            {
                // move head
                rope[0] += dir;
                for(var n = 1; n < rope.Length; n++)
                {
                    var headPos = rope[n-1];
                    var tailPos = rope[n];
                    if (Math.Max(Math.Abs(headPos.X - tailPos.X), Math.Abs(headPos.Y - tailPos.Y)) >= 2)
                    {
                        if (headPos.X == tailPos.X || headPos.Y == tailPos.Y)
                        {
                            // move tail towards head if 2 away
                            tailPos += ((headPos.X - tailPos.X) / 2,
                                        (headPos.Y - tailPos.Y) / 2);
                        }
                        else
                        {
                            // move diagonally towards head
                            tailPos += ((headPos.X > tailPos.X) ? 1 : -1,
                                        (headPos.Y > tailPos.Y) ? 1 : -1);
                        }
                    }
                    rope[n] = tailPos;
                }
                tailVisited.Add(rope.Last());

            }
        }
        return tailVisited.Count;
    }
    long Part2(IEnumerable<string> input) => 0;

}
