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