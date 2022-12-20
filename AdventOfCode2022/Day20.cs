﻿namespace AdventOfCode2022;

public class Day20 : ISolver
{
    public (string, string) ExpectedResult => ("11616", "9937909178485");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public long Part1(IEnumerable<string> input)
    {
        var numbers = input.Select(long.Parse).ToList();
        var allNodes = ToLinkedList(numbers).ToList();
        var zeroNode = allNodes.Single(n => n.Value == 0);
        foreach (var originalNode in allNodes)
        {
            var moveSteps = (int) Math.Abs(originalNode.Value % (allNodes.Count - 1));
            originalNode.Move(originalNode.Value >= 0 ? moveSteps : moveSteps * -1);
            //Console.WriteLine(string.Join(',', allNodes[0].Enumerate().Select(n => n.Value)));
        }
        // lazy!
        var val1 = zeroNode.Skip(1000).Value;
        var val2 = zeroNode.Skip(2000).Value;
        var val3 = zeroNode.Skip(3000).Value;
        return val1 + val2 + val3;
    }

    public long Part2(IEnumerable<string> input)
    {
        var decryptionKey = 811589153;
        var numbers = input.Select(n => long.Parse(n) * decryptionKey).ToList();
        var allNodes = ToLinkedList(numbers).ToList();
        var zeroNode = allNodes.Single(n => n.Value == 0);
        for (var n = 0; n < 10; n++)
        {
            foreach (var originalNode in allNodes)
            {
                var moveSteps = (int) Math.Abs(originalNode.Value % (allNodes.Count - 1));
                originalNode.Move(originalNode.Value >= 0 ? moveSteps : moveSteps * -1);
                //Console.WriteLine(string.Join(',', allNodes[0].Enumerate().Select(n => n.Value)));
            }
        }
        // lazy!
        var val1 = zeroNode.Skip(1000).Value;
        var val2 = zeroNode.Skip(2000).Value;
        var val3 = zeroNode.Skip(3000).Value;
        return val1 + val2 + val3;
    }


    private static IEnumerable<Node<long>> ToLinkedList(List<long> numbers)
    {
        List<Node<long>> allNodes = new List<Node<long>>();
        Node<long>? prev = null;
        foreach (var number in numbers)
        {
            var newNode = new Node<long>(number, prev);
            allNodes.Add(newNode);
            if (prev != null) { prev.Next = newNode; }
            prev = newNode;
        }
        prev!.Next = allNodes[0]; // make it circular
        allNodes[0].Prev = prev;
        return allNodes;
    }

    class Node<T>
    {
        public Node<T> Skip(int steps)
        {
            var target = this;
            for (var n = 0; n < Math.Abs(steps); n++)
            {
                target = steps > 0 ? target.Next : target.Prev;
            }
            return target;
        }

        public void Move(int steps)
        {
            if (steps == 0) return;
            var startingPoint = steps > 0 ? this.Prev : this.Next;
            // take myself out
            Remove();

            var target = startingPoint.Skip(steps);

            // add myself in
            if (steps > 0) InsertAfter(target);  
            else InsertBefore(target);
        }


        public IEnumerable<Node<T>> Enumerate()
        {
            var t = this;
            do
            {
                yield return t;
                t = t.Next;
            } while (t != this);
        }

        public void InsertAfter(Node<T> target)
        {
            this.Next = target.Next;
            this.Prev = target;

            target.Next.Prev = this;
            target.Next = this;
        }

        public void InsertBefore(Node<T> target)
        {
            this.Next = target;
            this.Prev = target.Prev;

            target.Prev.Next = this;
            target.Prev = this;
        }

        public void Remove()
        {
            // take myself out
            this.Prev.Next = this.Next;
            this.Next.Prev = this.Prev;
            this.Prev = null;
            this.Next = null;
        }

        public Node(T value, Node<T>? prev)
        {
            Value = value;
            Prev = prev;
        }

        public Node<T> Prev { get; set; }
        public Node<T> Next { get; set; }
        public T Value { get; }
    }


}
