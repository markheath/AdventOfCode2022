using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.RegularExpressions;
using SuperLinq;

namespace AdventOfCode2022;

public class Day16 : ISolver
{
    public (string, string) ExpectedResult => ("1716", "2504");

    public (string, string) Solve(string[] input)
    {
        //return Better(input);
        var valves = ParseValves(input);
        var sw = Stopwatch.StartNew();        
        var part1 = FindMaxFlow2(valves, new HashSet<string>() { "AA" }, ("AA", 30), ("XXX", 0));
        Console.WriteLine($"Part1 {part1} in {sw.Elapsed}");
        sw.Restart();
        var part2 = FindMaxFlow2(valves, new HashSet<string>() { "AA" }, ("AA", 26), ("AA", 26));
        Console.WriteLine($"Part2 {part2} in {sw.Elapsed}");

        return ($"{part1}", $"{part2}");
    }
    record Valve(string Name, int FlowRate, Dictionary<string,int> Destinations);

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

    Dictionary<string, int> cache = new();

    //int bestSoFar = 0;
    int FindMaxFlow2(IDictionary<string, Valve> valves, HashSet<string> openedValves, (string Location,int TimeRemaining)p0, (string Location, int TimeRemaining) p1)
    {
        var cacheKey = $"{string.Join(',', openedValves.Order())},{p0},{p1}";
        if (cache.ContainsKey(cacheKey))
        {
            //Console.WriteLine($"HIT: {cacheKey}");
            return cache[cacheKey];
        }
        // deal with player who has most time left first
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
            //if (max > bestSoFar)
            //{ bestSoFar = max; Console.WriteLine(bestSoFar); }
        }

        // note: might need this to be correct on some inputs - elephant's help isn't always what we want
        // if (other.pos != "XXX") max = Math.Max(FindMaxFlow2(valves, openedValves, ("XXX", 0), other), max);

        cache[cacheKey] = max;
        return max;
    }

    // since my day 16 solution still took 1.5mins to run, thought I'd learn from someone else
    // this elegant Python solution (by 4HbQ) is not that dissimilar from my final approach
    // this one runs in about 20s
    // https://www.reddit.com/r/adventofcode/comments/zn6k1l/comment/j0fti6c/?utm_source=share&utm_medium=web2x&context=3
    public (string,string) Better(string[] input)
    {
        var regex = new Regex(@"Valve (\w+) .*=(\d*); .* valves? (.*)");
        var V = new HashSet<string>(); // valve names
        var F = new Dictionary<string, int>(); // flow
        var D = new DefaultDictionary<(string, string), int>(1000); // distances
        foreach (var match in input.Select(line => regex.Match(line)))
        {
            var v = match.Groups[1].Value;
            var f = match.Groups[2].Value;
            var us = match.Groups[3].Value;
            V.Add(v); // store node
            if (f != "0") F[v] = int.Parse(f); // store flow
            foreach (var u in us.Split(", "))
            {
                D[(u, v)] = 1; //  store dist
            }
        }

        // floyd-warshall
        foreach (var (k, i, j) in V.Cartesian(V, V, (a, b, c) => (a, b, c)))
            D[(i, j)] = Math.Min(D[(i, j)], D[(i, k)] + D[(k, j)]);

        //@functools.cache
        int Search(int t, Dictionary<string, int> cache, string u, 
            ImmutableHashSet<string>vs, bool e = false) 
        {
            var cacheKey = $"{t}:{u}:{string.Join("", vs)}:{e}";
            if (cache.TryGetValue(cacheKey, out var val)) return val;
            var max = 
                vs.Where(v => D[(u, v)] < t)
                   .Select(v => F[v] * (t - D[(u, v)] - 1) + Search(t - D[(u, v)] - 1, cache, v, vs.Remove(v), e))
                .Append(e ? Search(26, cache, "AA", vs) : 0 )
                .Max();
            cache[cacheKey] = max;
            return max;
            // how part 2 works: we're essentially solving for 1 player, but after every valve, working out what would happen if the elephant did all the rest of them (starting from beginning), resulting in all possible partitionings of valves between player and elephant
        }
        var p1 = Search(30, new(), "AA", ImmutableHashSet<string>.Empty.Union(F.Keys));
        var p2 = Search(26, new(), "AA", ImmutableHashSet<string>.Empty.Union(F.Keys), e: true);
        return (p1.ToString(), p2.ToString());

        /* import collections as c, itertools, functools, re

 r = r'Valve (\w+) .*=(\d*); .* valves? (.*)'

 V, F, D = set(), dict(), c.defaultdict(lambda: 1000)

 for v, f, us in re.findall(r, open('in.txt').read()):
     V.add(v)                                  # store node
     if f != '0': F[v] = int(f)                # store flow
     for u in us.split(', '): D[u,v] = 1       # store dist

 for k, i, j in itertools.product(V, V, V):    # floyd-warshall
     D[i,j] = min(D[i,j], D[i,k] + D[k,j])

 @functools.cache
 def search(t, u='AA', vs=frozenset(F), e=False):
     return max(
        
    [F[v] * (t-D[u,v]-1) + search(t-D[u,v]-1, v, vs-{v}, e) for v in vs if D[u,v]<t] 
        + [search(26, vs=vs) if e else 0])

 print(search(30), search(26, e=True))
        */
    }
}
