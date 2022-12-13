﻿using System.Text;

namespace AdventOfCode2022;

public class Day13 : ISolver
{
    public (string, string) ExpectedResult => ("4643", "21614");



    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public class PacketItem 
    {

    };
    public class IntPacketItem : PacketItem
    {
        public IntPacketItem(string number)
        {
            Number = int.Parse(number);
        }

        public int Number { get; }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
    public class ListPacketItem : PacketItem
    {
        public ListPacketItem()
        {
            List = new List<PacketItem>();
        }

        public List<PacketItem> List { get;  }


        public int Compare(ListPacketItem other) // right order is 1 (true), wrong order is -1 (false)
        {
            for(var index = 0; index < this.List.Count; index++)
            {
                var left = this.List[index];
                // If the right list runs out of items first, the inputs are not in the right order.
                if (index >= other.List.Count) return -1;
                var right = other.List[index];
                //Console.WriteLine($"  - Compare {left} vs {right}");

                if (left is IntPacketItem leftInt && right is IntPacketItem rightInt)
                {
                    // If both values are integers, the lower integer should come first.
                    // If the left integer is lower than the right integer, the inputs are in the right order.

                    if (leftInt.Number < rightInt.Number)
                    {
                        //Console.WriteLine($"  - Left side is smaller, so inputs are in the right order\r\n");
                        return 1;
                    }
                    // If the left integer is higher than the right integer, the inputs are not in the right order.
                    if (leftInt.Number > rightInt.Number)
                    {
                        //Console.WriteLine($"  - Right side is smaller, so inputs are not in the right order\r\n");
                        return -1;
                    }
                    // Otherwise, the inputs are the same integer; continue checking the next part of the input

                }
                else if (left is ListPacketItem leftList && right is ListPacketItem rightList)
                {
                    // If both values are lists, compare the first value of each list, then the second value, and so on.
                    var result = leftList.Compare(rightList);
                    if (result != 0) return result;
                }
                else 
                {
                    // If exactly one value is an integer, convert the integer to a list which contains that integer as its only value, then retry the comparison.
                    if (left is IntPacketItem) // right must be a list packet
                    {
                        var leftList2 = new ListPacketItem();
                        leftList2.List.Add(left);
                        var result = leftList2.Compare((ListPacketItem)right);
                        if (result != 0) return result;
                    }
                    else // left is a list, right is a number
                    {
                        var rightList2 = new ListPacketItem();
                        rightList2.List.Add((IntPacketItem)right);
                        var result = ((ListPacketItem)left).Compare(rightList2);
                        if (result != 0) return result;
                    }
                }
                
            }
            if (this.List.Count == other.List.Count) return 0; // lists can be a tie

            // If the left list runs out of items first, the inputs are in the right order.
            //Console.WriteLine("Left side ran out of items, so inputs are in the right order");
            return 1; 
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach(var item in List)
            {
                sb.Append(item.ToString());
                sb.Append(",");
            }
            if (List.Count > 0) sb.Length--;
            sb.Append("]");
            return sb.ToString();
        }

    }


    public ListPacketItem Parse(string s)
    {
        ListPacketItem? topLevel = null;
        var listStack = new Stack<ListPacketItem>();
        var currentNum = "";
        foreach(var c in s)
        {
            if (c == '[')
            {
                var newList = new ListPacketItem();
                if(topLevel == null) topLevel= newList;
                else listStack.Peek().List.Add(newList);
                listStack.Push(newList);
            }
            else if (c == ']')
            {
                if (currentNum.Length > 0)
                {
                    listStack.Peek().List.Add(new IntPacketItem(currentNum));
                    currentNum = "";
                }
                listStack.Pop();
            }
            else if (c == ',')
            {
                if (currentNum.Length > 0)
                {
                    listStack.Peek().List.Add(new IntPacketItem(currentNum));
                    currentNum = "";
                }
            }
            else
            {
                currentNum += c;
            }
        }
        if (topLevel == null) throw new InvalidOperationException("No packet found");
        return topLevel;
    }

    long Part1(IEnumerable<string> input)
    {
        return input.Chunk(3).Select((chunk, index) => new { Index = index + 1, First = Parse(chunk[0]), Second = Parse(chunk[1]) })
            .Where(p => p.First.Compare(p.Second) == 1).Sum(p => p.Index);
    }
    long Part2(IEnumerable<string> input)
    {
        var allPackets = input.Where(s => !String.IsNullOrEmpty(s)).Select(Parse).ToList();
        var divider1 = Parse("[[2]]");
        var divider2 = Parse("[[6]]");
        allPackets.Add(divider1);
        allPackets.Add(divider2);
        allPackets.Sort(new Comparison<ListPacketItem>((a,b) => b.Compare(a)));
        return (allPackets.IndexOf(divider1) + 1) * (allPackets.IndexOf(divider2) + 1);
    }

}
