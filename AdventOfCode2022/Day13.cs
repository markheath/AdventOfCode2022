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
        private readonly int number;
        public int Number { get { if (!IsNumber) throw new InvalidOperationException("Not a number"); return number; }  }
        private bool IsNumber { get { return childPackets == null; } }
        private readonly List<PacketItem>? childPackets;
        public IReadOnlyList<PacketItem> List {  get { if (childPackets == null) throw new InvalidCastException("Not a list"); return childPackets; } }
        
        public void Add(PacketItem item) { if (childPackets == null) throw new InvalidCastException("Not a list"); childPackets.Add(item); }

        public PacketItem()
        {
            childPackets = new List<PacketItem>();
        }

        public PacketItem(PacketItem child) : this()
        {
            Add(child);
        }

        public PacketItem(string numberValue)
        {
            number = int.Parse(numberValue);
        }

        public static PacketItem Parse(string s)
        {
            return Parse(s, 1).Item1;
        }

        private static (PacketItem, int) Parse(string s, int startPos)
        {
            PacketItem list = new PacketItem();
            var currentNum = "";
            for (var n = startPos; n < s.Length; n++)
            {
                var c = s[n];
                if (c == '[')
                {
                    var (newList,newPos) = Parse(s, n + 1);
                    list.Add(newList);
                    n = newPos;
                }
                else if (c == ']' || c == ',')
                {
                    if (currentNum.Length > 0)
                    {
                        list.Add(new PacketItem(currentNum));
                        currentNum = "";
                    }
                    if (c == ']')
                    {
                        return (list, n);
                    }
                }
                else
                {
                    currentNum += c;
                }
            }
            return (list,s.Length);
        }

        public override string ToString()
        {
            if (IsNumber) return number.ToString();
            return $"[{string.Join(',', List)}]";
        }

        public int CompareTo(PacketItem? other) // right order is -1, wrong order is 1 
        {
            if (other == null || other.IsNumber) throw new InvalidOperationException("Only supporting compare with lists");

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
                        var result = new PacketItem(left).CompareTo(right);
                        if (result != 0) return result;
                    }
                    else // left is a list, right is a number
                    {
                        var result = left.CompareTo(new PacketItem(right));
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

    long Part1(IEnumerable<string> input)
    {
        return input.Chunk(3).Select((chunk, index) => new { Index = index + 1, First = PacketItem.Parse(chunk[0]), Second = PacketItem.Parse(chunk[1]) })
            .Where(p => p.First.CompareTo(p.Second) == -1).Sum(p => p.Index);
    }

    long Part2(IEnumerable<string> input)
    {
        var allPackets = input.Where(s => !String.IsNullOrEmpty(s)).Select(PacketItem.Parse).ToList();
        var divider1 = PacketItem.Parse("[[2]]");
        var divider2 = PacketItem.Parse("[[6]]");
        allPackets.Add(divider1);
        allPackets.Add(divider2);
        allPackets.Sort(); // uses the IComparable on PacketItem
        return (allPackets.IndexOf(divider1) + 1) * (allPackets.IndexOf(divider2) + 1);
    }
}
