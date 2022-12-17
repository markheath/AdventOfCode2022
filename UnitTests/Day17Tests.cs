using System;
using AdventOfCode2022;
using NUnit.Framework;
using static AdventOfCode2022.Day17;

namespace UnitTests;

public class Day17Tests
{
    private const string TestInput = @">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";


    [Test]
    public void Day17Part1TestInput()
    {
        var rockGrid = new RockGrid(TestInput);
        rockGrid.Run(2022);
        Assert.AreEqual(3068, rockGrid.HighestEmptyRow);
    }

    [Test]
    public void Day17Part2ExplorePeriodicity()
    {
        var rockGrid = new RockGrid(TestInput);
        var periodicity = TestInput.Length * 5;
        var highestEmpty = 0;
        var rocksDropped = 0;
        for (var n = 0; n < 20; n++)
        {
            rockGrid.RunSteps(periodicity);
            Console.WriteLine($"Run {n + 1}: {rockGrid.HighestEmptyRow - highestEmpty} rows added, {rockGrid.DroppedRocks - rocksDropped} rocks dropped");
            highestEmpty = rockGrid.HighestEmptyRow;
            rocksDropped = rockGrid.DroppedRocks;
        }
    }
    [Test]
    public void Day17Part2TestInput()
    {
        var periodicity = TestInput.Length * 5;

        var rockGrid = new RockGrid(TestInput);
        
        // do the first run which is special
        rockGrid.RunSteps(periodicity);
        var initialRows = rockGrid.HighestEmptyRow;
        var initialDroppedRocks = rockGrid.DroppedRocks;
        Console.WriteLine($"First period drops {initialDroppedRocks} rocks and adds {initialRows} rows");

        // do the next run which is repeating
        rockGrid.RunSteps(periodicity);
        var periodAddRows = rockGrid.HighestEmptyRow - initialRows;
        var periodDropRocks = rockGrid.DroppedRocks - initialDroppedRocks;
        Console.WriteLine($"Every period of {periodicity} drops {periodDropRocks} rocks and adds {periodAddRows} rows");

        // calculate the number of times the period must run
        var droppedRocksNeeded = 1000000000000 - initialDroppedRocks;
        var periodsNeeded = droppedRocksNeeded / periodDropRocks;
        Console.WriteLine($"Need {periodsNeeded} period");

        droppedRocksNeeded -= periodDropRocks * periodsNeeded;

        Console.WriteLine($"Need to drop another {droppedRocksNeeded} rocks after the periods");
        var before = rockGrid.HighestEmptyRow;
        rockGrid.Run((int)droppedRocksNeeded);
        var finalStageAddedRows = rockGrid.HighestEmptyRow - before;

        var totalRows = initialRows + periodAddRows * periodsNeeded + finalStageAddedRows;   

        var expected = 1514285714288;
        Assert.AreEqual(expected, totalRows);
    }

    [Test]
    public void Day17GridDrawing()
    {
        var rockGrid = new RockGrid(TestInput);
        Assert.AreEqual("|..@@@@.|\r\n|.......|\r\n|.......|\r\n|.......|\r\n+-------+", rockGrid.ToString());
        rockGrid.PushRock();
        Assert.AreEqual("|...@@@@|\r\n|.......|\r\n|.......|\r\n|.......|\r\n+-------+", rockGrid.ToString());
        rockGrid.DropRock();
        Assert.AreEqual("|...@@@@|\r\n|.......|\r\n|.......|\r\n+-------+", rockGrid.ToString());
        rockGrid.PushRock(); // can't go right
        Assert.AreEqual("|...@@@@|\r\n|.......|\r\n|.......|\r\n+-------+", rockGrid.ToString());
        rockGrid.DropRock();
        Assert.AreEqual("|...@@@@|\r\n|.......|\r\n+-------+", rockGrid.ToString());
        rockGrid.PushRock(); // another push right, doesn't move
        Assert.AreEqual("|...@@@@|\r\n|.......|\r\n+-------+", rockGrid.ToString());
        rockGrid.DropRock(); // reached bottom but not turned to rock yet
        Assert.AreEqual("|...@@@@|\r\n+-------+", rockGrid.ToString());
        rockGrid.PushRock(); // go left
        Assert.AreEqual("|..@@@@.|\r\n+-------+", rockGrid.ToString());
        rockGrid.DropRock();// can't go down, new rock appears (plus);
        Console.WriteLine(rockGrid);
        Assert.AreEqual("|...@...|\r\n|..@@@..|\r\n|...@...|\r\n|.......|\r\n|.......|\r\n|.......|\r\n|..####.|\r\n+-------+", rockGrid.ToString());
    }

}