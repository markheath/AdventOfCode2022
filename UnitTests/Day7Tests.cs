using System.ComponentModel;
using System.Drawing;
using System.IO;
using System;
using AdventOfCode2022;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using static AdventOfCode2022.Day7;
using System.Linq;

namespace UnitTests;

public class Day7Tests
{

    private const string TestInput = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

    [TestCase("e", 584)]
    [TestCase("a", 94853)]
    [TestCase("d", 24933642)]
    [TestCase("/", 48381165)]
    public void MeasureDirectorySize(string folderName, long expectedSize)
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day7();
        var root = solver.ParseInput(testInput);
        var dict = root.GetChildFolders().ToDictionary(f => f.Name, f => f.GetSize());
        dict[root.Name] = root.GetSize();
        Assert.That(dict[folderName], Is.EqualTo(expectedSize));
    }
    [Test]
    public void Part1()
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day7();
        var part1 = solver.Part1(testInput);

        Assert.That(part1, Is.EqualTo(95437));
    }

    [Test]
    public void Part2()
    {
        var testInput = TestInput.Split("\r\n");
        var solver = new Day7();
        var part2 = solver.Part2(testInput);

        Assert.That(part2, Is.EqualTo(24933642));
    }
}