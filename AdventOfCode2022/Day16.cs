using System.Text.RegularExpressions;
using SuperLinq;

namespace AdventOfCode2022;

public class Day16 : ISolver
{
    public (string, string) ExpectedResult => ("1716", "2504");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }
    record Valve(string Name, int FlowRate, Dictionary<string,int> Destinations);
    long Part1(IEnumerable<string> input)
    {
        var valves = ParseValves(input);

        int bestSoFar = 0;
        FindMaxFlow(valves, new HashSet<string>(), ref bestSoFar, "AA", 30, 0, "");
        return bestSoFar;
        //return 0; // first attempt took 50 minutes on test data and got 1650 (1 less than correct answer) 
    }

    // Dijkstra
    private int ShortestDistance(string start, string end, IDictionary<string, Valve> valves)
    {
        var shortestRoutes = new Dictionary<string, int>();
        var visited = new HashSet<string>();
        shortestRoutes[start] = 0;
        var queue = new List<string>
        {
            start
        };
        do
        {
            var node = queue.OrderBy(x => shortestRoutes[x]).First();
            queue.Remove(node);
            foreach (var connection in valves[node].Destinations.OrderBy(x => x.Value))
            {
                if (visited.Contains(connection.Key))
                    continue;
                if (!shortestRoutes.ContainsKey(connection.Key) ||
                    shortestRoutes[node] + connection.Value < shortestRoutes[connection.Key])
                {
                    shortestRoutes[connection.Key] = shortestRoutes[node] + connection.Value;
                    if (!queue.Contains(connection.Key))
                        queue.Add(connection.Key);
                }
            }
            visited.Add(node);
            if (node == end)
            {
                // add all shortest paths we've worked out
                foreach (var kvp in shortestRoutes)
                {
                    if (!valves[start].Destinations.ContainsKey(kvp.Key) && start != kvp.Key)
                    {
                        valves[start].Destinations[kvp.Key] = kvp.Value;
                        valves[kvp.Key].Destinations[start] = kvp.Value;
                    }
                }

                return shortestRoutes[end];
            }
        } while (queue.Any());
        throw new InvalidOperationException("end not found");
    }

    private static void PrintValves(IDictionary<string, Valve> valves)
    {
        foreach (var t in valves.Values)
        {
            Console.WriteLine($"{t.Name} {t.FlowRate} {string.Join(",", t.Destinations.Select(d => $"{d.Key}:{d.Value}"))}");
        }
    }

    IDictionary<string,Valve> EliminateValve(string valveName, IDictionary<string, Valve> dict)
    {
        var newDict = new Dictionary<string,Valve>(dict);
        var valve = dict[valveName];
        newDict.Remove(valveName);
        foreach (var dest in valve.Destinations)
        {
            var destValve = newDict[dest.Key];
            var newDestinations = new Dictionary<string,int>(destValve.Destinations);
            newDestinations.Remove(valveName); // all old destinations except the one we're removing

            // add the destinations from the eliminated node, except to self
            foreach (var kvp in valve.Destinations.Where(d => d.Key != dest.Key))
            {
                if (!newDestinations.ContainsKey(kvp.Key))
                    newDestinations.Add(kvp.Key, kvp.Value + dest.Value);
            }

            // replace
            newDict[dest.Key] = new Valve(destValve.Name, destValve.FlowRate, newDestinations);
        }
        return newDict;
    }

    void FindMaxFlow(IDictionary<string,Valve> valves, HashSet<string> openedValves, ref int bestSoFar, string currentPosition, int timeRemaining, int currentFlow, string journey)
    {
        if (timeRemaining <= 0)
        {
            bestSoFar = Math.Max(bestSoFar, currentFlow);
            return;
        }
        var currentValve = valves[currentPosition];
        if (currentValve.FlowRate > 0)
        {
            journey += $",opening {currentPosition} at minute {1 + 30 - timeRemaining}";
            openedValves = new HashSet<string>(openedValves);
            openedValves.Add(currentPosition);
            timeRemaining--; // time to open the valve
            currentFlow += timeRemaining * currentValve.FlowRate;
            if (timeRemaining <= 0)
            {
                bestSoFar = Math.Max(bestSoFar, currentFlow);
                return;
            }
        }
        var reachedEnd = true;
        foreach (var dest in currentValve.Destinations.Where(d => !openedValves.Contains(d.Key)))
        {
            reachedEnd = false;
            FindMaxFlow(valves, openedValves, ref bestSoFar, dest.Key, timeRemaining - dest.Value, currentFlow, journey);
        }
        if (reachedEnd)
        {
            // nowhere else to go - all valves are open
            //Console.WriteLine($"Finished {currentFlow} {journey}");
            bestSoFar = Math.Max(bestSoFar, currentFlow);
        }
    }

    private IDictionary<string,Valve> ParseValves(IEnumerable<string> input)
    {
        var tunnels = input.Select(l => Regex.Matches(l, "[A-Z][A-Z]").Select(m => m.Value).ToArray());
        var flowRates = input.Select(l => int.Parse(Regex.Match(l, "\\d+").Value)).ToArray();
        var allValves = tunnels.Zip(flowRates).Select(t => new Valve(t.First[0], t.Second, t.First.Skip(1).ToDictionary(d => d, d => 1)));
        IDictionary<string,Valve> valves = allValves.ToDictionary(v => v.Name, v => v);


        //PrintValves(valves);
        var toRemove = valves.Values.Where(x => x.FlowRate == 0 && x.Name != "AA").ToList();
        foreach (var v in toRemove)
        {
            //Console.WriteLine($"Eliminating {v.Name}:");
            valves = EliminateValve(v.Name, valves);
            //PrintValves(valves);
        }

        var allDests = valves.Keys.ToList();
        foreach (var from in allDests)
        {
            foreach (var to in allDests)
            {
                if (!valves[from].Destinations.ContainsKey(to))
                {
                    ShortestDistance(from, to, valves);
                }
            }
        }
        // fully mincosted graph:
        //Console.WriteLine("With all dests:");
        //PrintValves(valves);
        return valves;
    }

    long Part2(IEnumerable<string> input)
    {
        var valves = ParseValves(input);
        // start with AA as an already opened valve
        return FindMaxFlow2(valves, new HashSet<string>() { "AA" }, ("AA",26), ("AA",26));
    }

    int bestSoFar = 0;
    int FindMaxFlow2(IDictionary<string, Valve> valves, HashSet<string> openedValves, (string Location,int TimeRemaining)p0, (string Location, int TimeRemaining) p1)
    {
        // deal with player who has most time lieft first
        var (maxP, other) = (p0.TimeRemaining >= p1.TimeRemaining) ? (p0, p1) : (p1, p0);
        // what valves does this player still have time to open?
        var max = 0;
        foreach(var dest in valves[maxP.Location].Destinations
                .Where(d => !openedValves.Contains(d.Key))
                .Where(d => d.Value + 1 <= maxP.TimeRemaining))
        {
            var timeLeft = (maxP.TimeRemaining - dest.Value - 1);
            var destFlow = valves[dest.Key].FlowRate * timeLeft;
            var extraFlow = FindMaxFlow2(valves, new HashSet<string>(openedValves) { dest.Key }, (dest.Key, timeLeft), other);
            max = Math.Max(max, destFlow + extraFlow);
            if (max > bestSoFar)
            { bestSoFar = max; Console.WriteLine(bestSoFar); }
        }
        return max;
    }

}

