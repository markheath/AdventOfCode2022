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