# Advent of Code 2022

This project contains solutions to the [Advent of Code](https://adventofcode.com/) puzzles for 2022 in C#. Not sure if I'll make it to the end as the puzzles typically get progressively harder, and I don't always have the time. I've based the project structure on last year's (and brought through some utils). Also updating to .NET 7 and using SuperLINQ this year.


Some notes:
- Day 1: nice easy one-liner with `GroupAdjacent` from SuperLINQ coming in handy
- Day 2: used dictionaries to work out winning and losing moves, but since the game is cyclic, could simplify this further with a modulo operation
- Day 3: another one-liner, this time trying to remind myself to use the C# string ranges feature instead of calling Substring
- Day 4: my overlap and contains methods could be simplified but overall a relatively straightforward one
- Day 5: solved this relatively straightforward with stacks, although they did need to be reversed so maybe there is a better data structure
- Day 6: another more or less one-liner, with SuperLINQ's `Window` and `TakeUntil` proving very helpful
- Day 7: the main challenge was writing the parser - I also chose to make a custom class to help with readability
- Day 8: I knew my `Grid` from last year would come in handy. I added a `LineOut` method and that was more or less enough to solve
- Day 9: this was a fun one. A hashset keeps track of visited positions and if you moved the calculate new tail position into a function, it remains quite readable, and easy to make generic enough to solve part 2
- Day 10: initially I tried to be clever and optimize this, but ended up with a simple Compute method returning an `IEnumerable<int>` to simplify the rest of the code. Didn't bother to implement OCR for my answer!
- Day 11: parsing the monkeys was a good use case for Regex. The worry reducer for part 2 took me a bit of time to come up with. I used `BigInteger` but maybe could have got away with a long.
- Day 12: another one where my Grid class came in really handy, although I made a few improvements. The big challenge was making the part 1 solution generic enough to also solve part 2 which takes it close to being a generic breadth-first algorithm (maybe useful later this year!)
- Day 13: Another one where the main challenge was getting the parsing right (a bit jealous of the Javascript programmers!). I made a few mistakes, and so after getting the right answer, refactored my code to how it ought to work, with recursive parsing, and implementing `CompareTo` for simple sorting.
- Day 14: I was really ill which didn't help matters, but this was a fun one. Parsing the input was simple - with hindsight I probably should have used a Dictionary approach rather than a grid, but I liked being able to easily print it out. I made a bunch of silly errors that slowed me down, and eventually part 2 came up with the solution but in 15 seconds! I realised there was a nice optimization where you could always inject the next grain of sand at most recent point where the previous grain had fallen straight down which resulted in a massive speed increase.
- Day 15: It was high time I made a range combiner, and part 2 forced me to do it. This meant I could fairly quickly work out which rows were fully excluded. However, my solution needs optimizing a lot more. I suspect that there are some input ranges so big that they massively reduce the search space and detecting those would help immensely.
- Day 16: A variation on the travelling salesman problem, and a chance to use Dijkstra's algorithm to calculate the shortest path. I eventually got part 1 working, and managed to make a few improvements and optimizations as preparation for part 2. Part 2 however proved a lot trickier than I hoped. After getting my answer working for the test input, my solver worked for the real input but incredibly slowly. I came back a couple of days later and rewrote my solution so that it worked in 3 minutes. However I think there is still a lot of room for improvement (and even a possible bug) so I'd ideally like to do another round before calling it a day.
- Day 17: A much less stressful one than yesterday. Part 1 was fun, and I built it up with a test-driven approach that worked well. For part 2 I realised I just needed to calculate the periodicity, although I found that it was easy to accidentally mix up rocks and rows in the calculations (F# unit of measure would protect against that!).
- Day 18: Another kind one. I updated my 3D coordinate to add a Neighbours method similar to my 2D coordinate and that allowed me to solve part 1 in almost one line. Part 2 was a simple case of implementing a 3D flood fill algorithm which was made easier by adding comparisons to my 3D coordinate class.
- Day 19: Very similar to day 16 - lots of depth first recursion with the need to memoize and find other optimizations to prevent the solution from taking forever. I was convinced that there ought to be a way of intelligently deciding what we needed to try getting more of, but couldn't seem to get it to work, so a simple approach to avoid "stockpiling" was the best I could come up with.
- Day 20: In one sense, a fairly straightforward doubly linked list problem, but as always a few details can trip you up, such as accounting for things moving through the entire list more than once (remove it before applying the modulo), and the fact that the same input number could appear, meaning you must track nodes in the list, not just values.
- Day 21: Part 1 was nice and easy - loop through the unsolved calculations until you can solve the target, but the approach I chose meant that part 2 needed some more thinking as I hadn't built up a tree of dependencies between the calculations. However, I realised that simply by rearranging all equations, part 1's solution would work just fine.
- Day 22: Part 1 was relatively easy - we just needed to fast-forward across empty zones as well as wrap round. However, part 2 upped the stakes somewhat, and I chose to rewrite part 1 to use "teleport" locations where you would jump to another part of the grid. This just meant that I needed to work out how to fold my cube. Initially that seemed too hard, so I hard-coded the edges that needed to be connected for my input. However, once I got that working, it's not hard to see that simply by naming the cube sides in your input (which is quite easy), you'd know what edges connected (because that's always the same). The only possible gotcha would be the rule that decides if an edge needs to be flipped or not - I don't know if the rule I used is generic for all layouts or only worked for mine.