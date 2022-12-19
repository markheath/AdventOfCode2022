using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day19 : ISolver
{
    public (string, string) ExpectedResult => ("1766", "");

    

    public int Part1(string blueprint, int minutes)
    {
        var costs = Regex.Matches(blueprint, "\\d+").Skip(1).Select(m => int.Parse(m.Value)).ToArray();
        var oreCost = (costs[0], 0, 0);
        var clayCost = (costs[1], 0, 0);
        var obsidianCost = (costs[2], costs[3], 0);
        var geodeCost = (costs[4], 0, costs[5]);
        return MaximumGeodes(oreCost, clayCost, obsidianCost, geodeCost, (1, 0, 0), (0, 0, 0), minutes, new());
    }



    public int MaximumGeodes(Coord3 oreCost, Coord3 clayCost, Coord3 obsidianCost, Coord3 geodeCost, Coord3 robots, Coord3 balance, int timeRemaining, Dictionary<string,int> cache)
    {
        var cacheKey = $"{robots},{balance},{timeRemaining}";
        if(cache.ContainsKey(cacheKey)) {  return cache[cacheKey]; }

        //Console.WriteLine($"NEW MINUTE {timeRemaining}; balance: {balance}; robots: {robots}");
        timeRemaining--;
        if (timeRemaining <= 0) return 0;
        var max = 0;
        // make new balance (but not usable until next go)
        var nextBalance = balance + robots;
        // always buy geodes if we can
        if (balance >= geodeCost)
        {
            //Console.WriteLine($"CAN MAKE GEODES {timeRemaining}");
            var geodesProduced = timeRemaining;
            max = Math.Max(max, geodesProduced + MaximumGeodes(oreCost, clayCost, obsidianCost, geodeCost, robots, nextBalance - geodeCost, timeRemaining, cache));
        }
        else
        {
            // Geode costs obsidian and ore
            var weNeedMoreObsidian = (geodeCost - balance).Z > 0;
            var weNeedMoreOre = (geodeCost - balance).X > 0;
            var weNeedMoreClay = false;

            if (balance >= obsidianCost)
            {
                //Console.WriteLine($"CAN MAKE OBSIDIAN");
                max = Math.Max(max, MaximumGeodes(oreCost, clayCost, obsidianCost, geodeCost, robots + (0, 0, 1), nextBalance - obsidianCost, timeRemaining, cache));
            }

            

            if (balance >= clayCost)
            {
                //Console.WriteLine($"CAN MAKE CLAY");
                max = Math.Max(max, MaximumGeodes(oreCost, clayCost, obsidianCost, geodeCost, robots + (0, 1, 0), nextBalance - clayCost, timeRemaining, cache));
            }
            if (balance >= oreCost)
            {
                //Console.WriteLine($"CAN MAKE ORE");
                max = Math.Max(max, MaximumGeodes(oreCost, clayCost, obsidianCost, geodeCost, robots + (1, 0, 0), nextBalance - oreCost, timeRemaining, cache));
            }

            // do nothing in this minute
            max = Math.Max(max, MaximumGeodes(oreCost, clayCost, obsidianCost, geodeCost, robots, nextBalance, timeRemaining, cache));
        }
        cache[cacheKey] = max;
        return max;
    }

    public (string, string) Solve(string[] input)
    {
        var part1 = input.Select((b,index) => new { Geodes = Part1(b, 24), Blueprint = index + 1 }).Sum(x => x.Geodes * x.Blueprint);
        return ($"{part1}", $"0");
    }


}
