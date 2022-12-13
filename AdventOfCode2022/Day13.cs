using System.Text;

namespace AdventOfCode2022;

public class Day13 : ISolver
{
    public (string, string) ExpectedResult => ("4643", "");



    public (string, string) Solve(string[] input)
    {
        /* testing the parser - seems fine
        var pairs = input.Chunk(3).Select((chunk, index) => new { Index = index + 1, First = Parse(chunk[0]), Second = Parse(chunk[1]) });
        var sb = new StringBuilder();
        foreach(var pair in pairs)
        {
            sb.AppendLine($"{pair.First}");
            sb.AppendLine($"{pair.Second}");
            sb.AppendLine();
        }
        File.WriteAllText(@"C:\Users\mheath\code\github\AdventOfCode2022\Input\Day13Parsed.txt", sb.ToString());*/


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
        public ListPacketItem(ListPacketItem parent)
        {
            List = new List<PacketItem>();
            Parent = parent;
        }

        public List<PacketItem> List { get;  }
        public ListPacketItem? Parent { get; }

        public int Compare(ListPacketItem other) // right order is 1 (true), wrong order is -1 (false)
        {
            for(var index = 0; index < this.List.Count; index++)
            {
                var left = this.List[index];
                // If the right list runs out of items first, the inputs are not in the right order.
                if (index >= other.List.Count) return -1;
                var right = other.List[index];
                Console.WriteLine($"  - Compare {left} vs {right}");

                if (left is IntPacketItem leftInt && right is IntPacketItem rightInt)
                {
                    // If both values are integers, the lower integer should come first.
                    // If the left integer is lower than the right integer, the inputs are in the right order.

                    if (leftInt.Number < rightInt.Number)
                    {
                        Console.WriteLine($"  - Left side is smaller, so inputs are in the right order\r\n");
                        return 1;
                    }
                    // If the left integer is higher than the right integer, the inputs are not in the right order.
                    if (leftInt.Number > rightInt.Number)
                    {
                        Console.WriteLine($"  - Right side is smaller, so inputs are not in the right order\r\n");
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
                        var leftList2 = new ListPacketItem(null);
                        leftList2.List.Add(left);
                        var result = leftList2.Compare((ListPacketItem)right);
                        if (result != 0) return result;
                    }
                    else // left is a list, right is a number
                    {
                        var rightList2 = new ListPacketItem(null);
                        rightList2.List.Add((IntPacketItem)right);
                        var result = ((ListPacketItem)left).Compare(rightList2);
                        if (result != 0) return result;
                    }
                }
                
            }
            if (this.List.Count == other.List.Count) return 0; // lists can be a tie

            // If the left list runs out of items first, the inputs are in the right order.
            Console.WriteLine("Left side ran out of items, so inputs are in the right order");
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

        ListPacketItem parent = new ListPacketItem(null);
        ListPacketItem current = parent;
        var currentNum = "";
        for(var n = 1; n < s.Length; n++)
        {
            if (s[n] == '[')
            {
                var newList = new ListPacketItem(current);
                current.List.Add(newList);
                current = newList;
            }
            else if (s[n] == ']')
            {
                if (currentNum.Length > 0)
                {
                    current.List.Add(new IntPacketItem(currentNum));
                    currentNum = "";
                }
                current = current.Parent;
            }
            else if (s[n] == ',')
            {
                if (currentNum.Length > 0)
                {
                    current.List.Add(new IntPacketItem(currentNum));
                    currentNum = "";
                }
            }
            else
            {
                currentNum += s[n];
            }
        }
        return parent;
    }

    long Part1(IEnumerable<string> input)
    {
        return input.Chunk(3).Select((chunk, index) => new { Index = index + 1, First = Parse(chunk[0]), Second = Parse(chunk[1]) })
            .Where(p => p.First.Compare(p.Second) == 1).Sum(p => p.Index);
    }
    long Part2(IEnumerable<string> input)
    {
        return 0;
    }

}
