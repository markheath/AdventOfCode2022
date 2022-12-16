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


        PrintValves(valves);
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
        var bestSoFar = 0;
        FindMaxFlow2(valves, new HashSet<string>() { "AA" }, ref bestSoFar, new[] { ("AA",0), ("AA",0) }, 26, 0, "");
        return bestSoFar;
    }

    void FindMaxFlow2(IDictionary<string, Valve> valves, HashSet<string> openedValves, ref int bestSoFar, (string,int)[] players, int timeRemaining, int currentFlow, string journey)
    {
        if (timeRemaining <= 0)
        {
            if (currentFlow > bestSoFar) Console.WriteLine($"New high score {currentFlow}");
            bestSoFar = Math.Max(bestSoFar, currentFlow);
            return;
        }
        var t = 26 - timeRemaining + 1;
        var possibleDests = players.Select(_ => new List<(string,int)>()).ToList();
        var avoidDests = new HashSet<string>();
        // decide what they will do next if they are not en route
        for (var p = 0; p < players.Length; p++)
        {
            var (currentValveName, arrivalTime) = players[p];
            if (arrivalTime < 0) throw new InvalidOperationException();
            if (arrivalTime == 0)
            {
                var currentValve = valves[currentValveName];
                if (!openedValves.Contains(currentValveName))
                {
                    journey += $"{t}: p[{p}] opens {currentValveName}, ";
                    openedValves = new HashSet<string>(openedValves) { currentValveName };
                    currentFlow += (timeRemaining-1) * currentValve.FlowRate;
                    possibleDests[p].Add((currentValveName, 0)); // we'll "arrive" here again
                    avoidDests.Add(currentValveName);
                }
                else
                {                     
                    // we can only choose destinations when we know what the other player is doing
                }
            }
            else
            {
                possibleDests[p].Add((currentValveName, arrivalTime-1));  // getting closer
                avoidDests.Add(currentValveName);
            }
        }

        for (var p = 0; p < players.Length; p++)
        {
            var (currentValveName, arrivalTime) = players[p];
            if (possibleDests[p].Count == 0)
            {
                var currentValve = valves[currentValveName];
                // valve is already open here, let's pick somewhere else to go
                possibleDests[p].AddRange(currentValve.Destinations
                    .Where(d => !openedValves.Contains(d.Key))
                    .Where(d => !avoidDests.Contains(d.Key))
                    .OrderByDescending(d => valves[d.Key].FlowRate * (timeRemaining-d.Value-1))
                    .Select(d => (d.Key, d.Value - 1))); // have already used this second to move towards this square
            }
        }

        var reachedEnd = true;
        if (possibleDests[0].Count == 1 && possibleDests[1].Count == 0) {
            possibleDests[1].Add(("XXXX", 1000));
        }
        else if (possibleDests[1].Count == 1 && possibleDests[0].Count == 0)
        {
            possibleDests[0].Add(("XXXX", 1000));
        }

        // try every possible pair of next actions, but don't both go to open the same valve if we're moving on the same turn
        foreach (var possible in possibleDests[0].Cartesian(possibleDests[1], (x, y) => (x,y)).Where(p => p.x.Item1 != p.y.Item1))
        {
            reachedEnd = false;
            FindMaxFlow2(valves, new HashSet<string>(openedValves), ref bestSoFar, new(string,int)[] { possible.x, possible.y }, timeRemaining - 1, currentFlow, journey);
            //break;
        }
        if (reachedEnd)
        {
            // nowhere else to go - all valves are open
            //Console.WriteLine($"Finished {currentFlow} {journey}");
            if (currentFlow > bestSoFar) Console.WriteLine($"New high score {currentFlow}");

            bestSoFar = Math.Max(bestSoFar, currentFlow);
        }
    }

}

