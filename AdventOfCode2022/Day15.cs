using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day15 : ISolver
{
    public (string, string) ExpectedResult => ("", "");// 4627827 too low 

    public int Part1(string[] testInput, int row)
    {
        var sensors = testInput.Select(s => Regex.Matches(s,@"\-?\d+").Select(m => int.Parse(m.Value)).ToArray())
            .Select(m => new { Sensor = new Coord(m[0], m[1]), ClosestBeacon = new Coord(m[2], m[3]) })
            .ToList();
        var ruledOut = new HashSet<int>();
        foreach(var sensor in sensors)
        {
            var manhattenDistance = sensor.Sensor.ManhattenDistanceTo(sensor.ClosestBeacon);
            var distanceFromRow = Math.Abs(row - sensor.Sensor.Y);
            if (distanceFromRow <= manhattenDistance)
            {
                var rangeStart = sensor.Sensor.X - (manhattenDistance - distanceFromRow);
                var rangeEnd = sensor.Sensor.X + (manhattenDistance - distanceFromRow);
                for(var n = rangeStart ; n <= rangeEnd; n++) {
                    ruledOut.Add(n);
                }
            }
        }
        foreach(var b in sensors.Select(s => s.ClosestBeacon).Where(b => b.Y == row))
        {
            ruledOut.Remove(b.X);
        }
        return ruledOut.Count;
    }

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input, 2000000)}", $"{Part2(input)}");
    }

    long Part2(IEnumerable<string> input) => 0;

}
