using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day16 : ISolver
{
    public (string, string) ExpectedResult => ("", "");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }
    record Route(string Target, int Distance);
    record Valve(string Name, int FlowRate, Route[] Destinations);
    long Part1(IEnumerable<string> input)
    {
        var valves = ParseValves(input);
        PrintValves(valves);
        var toRemove = valves.Values.Where(x => x.FlowRate == 0 && x.Name != "AA").ToList();
        foreach (var v in toRemove)
        {
            Console.WriteLine($"Eliminating {v.Name}:");
            valves = EliminateValve(v.Name, valves);
            PrintValves(valves);
        }

        return FindMaxFlow(valves, "AA", 30, 0, "").Max();
        //return 0; // first attempt took 50 minutes on test data and got 1650 (1 less than correct answer) 
    }

    private static void PrintValves(IDictionary<string, Valve> valves)
    {
        foreach (var t in valves.Values)
        {
            Console.WriteLine($"{t.Name} {t.FlowRate} {string.Join(",", t.Destinations.Select(d => $"{d.Target}:{d.Distance}"))}");
        }
    }

    IDictionary<string,Valve> EliminateValve(string valveName, IDictionary<string, Valve> dict)
    {
        var newDict = new Dictionary<string,Valve>(dict);
        var valve = dict[valveName];
        newDict.Remove(valveName);
        foreach (var dest in valve.Destinations)
        {
            var destValve = newDict[dest.Target];
            var newDestinations = new List<Route>(destValve.Destinations.Where(x => x.Target != valveName)); // all old destinations except the one we're removing
            // add the destinations from the eliminated node, except to self
            newDestinations.AddRange(valve.Destinations.Where(d => d.Target != dest.Target).Select(d => new Route(d.Target, d.Distance + dest.Distance)));

            // replace
            newDict[dest.Target] = new Valve(destValve.Name, destValve.FlowRate, newDestinations.ToArray());
        }
        return newDict;
    }

    IEnumerable<int> FindMaxFlow(IDictionary<string,Valve> valves, string currentPosition, int timeRemaining, int currentFlow, string journey)
    {
        if (timeRemaining <= 0)
        {
            yield return currentFlow;
            yield break;
        }
        var currentValve = valves[currentPosition];
        if(currentValve.FlowRate > 0)
        {
            journey += $",opening {currentPosition} at minute {1+30-timeRemaining}";
            timeRemaining--; // time to open the valve
            currentFlow += timeRemaining * currentValve.FlowRate;
            if (timeRemaining <= 0)
            {
                yield return currentFlow;
                yield break;
            }
        }
        valves = EliminateValve(currentPosition, valves);   
        if (currentValve.Destinations.Length > 0) 
        {
            // .OrderByDescending(d => valves[d.Target].FlowRate * (timeRemaining - d.Distance))
            foreach (var dest in currentValve.Destinations)
            {
                foreach (var flow in FindMaxFlow(valves, dest.Target, timeRemaining - dest.Distance, currentFlow,journey))
                {
                    yield return flow;
                }
            }
        }
        else
        {
            // nowhere else to go - all valves are open
            Console.WriteLine($"Finished {currentFlow} {journey}");
            yield return currentFlow;
        }
    }

    private static IDictionary<string,Valve> ParseValves(IEnumerable<string> input)
    {
        var tunnels = input.Select(l => Regex.Matches(l, "[A-Z][A-Z]").Select(m => m.Value).ToArray());
        var flowRates = input.Select(l => int.Parse(Regex.Match(l, "\\d+").Value)).ToArray();
        var valves = tunnels.Zip(flowRates).Select(t => new Valve(t.First[0], t.Second, t.First.Skip(1).Select(d => new Route(d,1)).ToArray()));
        return valves.ToDictionary(v => v.Name, v => v);
    }

    long Part2(IEnumerable<string> input) => 0;

}