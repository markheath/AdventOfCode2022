using System.Text;

namespace AdventOfCode2022;

public class Day13 : ISolver
{
    public (string, string) ExpectedResult => ("4643", "21614");



    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public class PacketItem : IComparable<PacketItem>
    {
        private int number;
        public int Number { get { if (!IsNumber) throw new InvalidOperationException("Not a number"); return number; }  }
        private bool IsNumber { get { return childPackets == null; } }
        private readonly List<PacketItem>? childPackets;
        public IReadOnlyList<PacketItem> List {  get { if (childPackets == null) throw new InvalidCastException("Not a list"); return childPackets; } }
        
        public void Add(PacketItem item) { childPackets.Add(item); }

        public PacketItem()
        {
            childPackets = new List<PacketItem>();
        }

        public PacketItem(string numberValue)
        {
            number = int.Parse(numberValue);
        }

        public override string ToString()
        {
            if (IsNumber) return number.ToString();

            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in List)
            {
                sb.Append(item.ToString());
                sb.Append(",");
            }
            if (List.Count > 0) sb.Length--;
            sb.Append("]");
            return sb.ToString();
        }

        public int CompareTo(PacketItem? other) // right order is -1, wrong order is 1 
        {
            if (other.IsNumber) throw new InvalidOperationException("Only supporting compare with lists");

            for (var index = 0; index < this.List.Count; index++)
            {
                var left = this.List[index];
                // If the right list runs out of items first, the inputs are not in the right order.
                if (index >= other.List.Count) return 1;
                var right = other.List[index];
                //Console.WriteLine($"  - Compare {left} vs {right}");

                if (left.IsNumber && right.IsNumber)
                {
                    // If both values are integers, the lower integer should come first.
                    // If the left integer is lower than the right integer, the inputs are in the right order.

                    if (left.Number < right.Number)
                    {
                        //Console.WriteLine($"  - Left side is smaller, so inputs are in the right order\r\n");
                        return -1;
                    }
                    // If the left integer is higher than the right integer, the inputs are not in the right order.
                    if (left.Number > right.Number)
                    {
                        //Console.WriteLine($"  - Right side is smaller, so inputs are not in the right order\r\n");
                        return 1;
                    }
                    // Otherwise, the inputs are the same integer; continue checking the next part of the input

                }
                else if (!left.IsNumber && !right.IsNumber)
                {
                    // If both values are lists, compare the first value of each list, then the second value, and so on.
                    var result = left.CompareTo(right);
                    if (result != 0) return result;
                }
                else
                {
                    // If exactly one value is an integer, convert the integer to a list which contains that integer as its only value, then retry the comparison.
                    if (left.IsNumber) // right must be a list packet
                    {
                        var leftList2 = new PacketItem();
                        leftList2.Add(left);
                        var result = leftList2.CompareTo(right);
                        if (result != 0) return result;
                    }
                    else // left is a list, right is a number
                    {
                        var rightList2 = new PacketItem();
                        rightList2.Add(right);
                        var result = left.CompareTo(rightList2);
                        if (result != 0) return result;
                    }
                }

            }
            if (this.List.Count == other.List.Count) return 0; // lists can be a tie

            // If the left list runs out of items first, the inputs are in the right order.
            //Console.WriteLine("Left side ran out of items, so inputs are in the right order");
            return -1;
        }
    }

    public PacketItem Parse(string s)
    {
        PacketItem? topLevel = null;
        var listStack = new Stack<PacketItem>();
        var currentNum = "";
        foreach(var c in s)
        {
            if (c == '[')
            {
                var newList = new PacketItem();
                if(topLevel == null) topLevel= newList;
                else listStack.Peek().Add(newList);
                listStack.Push(newList);
            }
            else if (c == ']' || c == ',')
            {
                if (currentNum.Length > 0)
                {
                    listStack.Peek().Add(new PacketItem(currentNum));
                    currentNum = "";
                }
                if (c == ']')
                {
                    listStack.Pop();
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
            .Where(p => p.First.CompareTo(p.Second) == -1).Sum(p => p.Index);
    }
    long Part2(IEnumerable<string> input)
    {
        var allPackets = input.Where(s => !String.IsNullOrEmpty(s)).Select(Parse).ToList();
        var divider1 = Parse("[[2]]");
        var divider2 = Parse("[[6]]");
        allPackets.Add(divider1);
        allPackets.Add(divider2);
        allPackets.Sort();
        return (allPackets.IndexOf(divider1) + 1) * (allPackets.IndexOf(divider2) + 1);
    }

}
