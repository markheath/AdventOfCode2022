namespace AdventOfCode2022;

public class Day20 : ISolver
{
    public (string, string) ExpectedResult => ("11616", "9937909178485");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public long Part1(IEnumerable<string> input)
    {
        var allNodes = LinkedListNode<long>.ToLinkedList(input.Select(long.Parse)).Enumerate().ToList();
        var zeroNode = allNodes.Single(n => n.Value == 0);
        Mix(allNodes);
        return SumCoordinates(zeroNode);
    }

    private static void Mix(ICollection<LinkedListNode<long>> allNodes)
    {
        foreach (var originalNode in allNodes)
        {
            var moveSteps = (int)Math.Abs(originalNode.Value % (allNodes.Count - 1));
            originalNode.Move(originalNode.Value >= 0 ? moveSteps : moveSteps * -1);
            //Console.WriteLine(string.Join(',', allNodes[0].Enumerate().Select(n => n.Value)));
        }
    }

    public long Part2(IEnumerable<string> input)
    {
        var decryptionKey = 811589153;
        var allNodes = LinkedListNode<long>.ToLinkedList(input.Select(n => long.Parse(n) * decryptionKey)).Enumerate().ToList();
        var zeroNode = allNodes.Single(n => n.Value == 0);
        for (var n = 0; n < 10; n++)
        {
            Mix(allNodes);
        }
        return SumCoordinates(zeroNode);
    }

    private static long SumCoordinates(LinkedListNode<long> zeroNode)
    {
        long sum = 0;
        for (var n = 0; n < 3; n++)
        {
            zeroNode = zeroNode.Skip(1000);
            sum += zeroNode.Value;
        }
        return sum;
    }
}
