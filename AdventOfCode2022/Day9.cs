namespace AdventOfCode2022;

public class Day9 : ISolver
{
    public (string, string) ExpectedResult => ("", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    long Part1(IEnumerable<string> input)
    {
        var headPos = new Coord(0, 0);
        var tailPos = new Coord(0, 0);
        var tailVisited = new HashSet<Coord>();
        tailVisited.Add(tailPos);
        foreach (var inst in input)
        {
            var dir = inst[0] switch { 'R' => (1, 0), 'L' => (-1, 0), 'U' => (0, 1), 'D' => (0, -1) };
            var count = int.Parse(inst[2..]);
            for(var step = 0; step < count; step++)
            {
                headPos += dir;
                if(Math.Max(Math.Abs(headPos.X - tailPos.X), Math.Abs(headPos.Y - tailPos.Y))>=2)
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
                    tailVisited.Add(tailPos);
                }
            }
        }
        return tailVisited.Count;
    }
    long Part2(IEnumerable<string> input) => 0;

}
