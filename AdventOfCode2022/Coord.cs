using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022;

public readonly struct Coord : IEnumerable<int>, IEquatable<Coord>
{
    private readonly int x;
    private readonly int y;
    public int X { get => x; }
    public int Y { get => y; }

    public int this[int index]
    {
        get { return index == 0 ? x : y; }
    }

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator (int, int)(Coord c) => (c.x, c.y);
    public static implicit operator Coord((int X, int Y) c) => new Coord(c.X, c.Y);

    public void Deconstruct(out int x, out int y)
    {
        x = this.x;
        y = this.y;
    }

    public static Coord operator +(Coord a, Coord b)
    {
        return new Coord(a.x + b.x, a.y + b.y);
    }

    public static Coord operator -(Coord a, Coord b)
    {
        return new Coord(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Coord a, Coord b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Coord a, Coord b)
    {
        return !a.Equals(b);
    }

    private static readonly IEnumerable<Coord> horizontalNeighbours = new[] { new Coord(-1, 0), new Coord(1, 0), new Coord(0, -1), new Coord(0, 1) };
    private static readonly IEnumerable<Coord> diagonalNeigbours = new[] { new Coord(-1, -1), new Coord(1, 1), new Coord(1, -1), new Coord(-1, 1) };

    public IEnumerable<Coord> Neighbours(bool includeDiagonals = false)
    {
        var neighbours = includeDiagonals ? horizontalNeighbours.Concat(diagonalNeigbours) : horizontalNeighbours;
        var p = this;
        return neighbours.Select(n => p + n);
    }

    public IEnumerable<Coord> LineTo(Coord other)
    {
        var current = this;
        yield return current;
        while (current != other)
        { 
            current -= (current.X.CompareTo(other.X), current.Y.CompareTo(other.Y));
            yield return current;
        }
    }

    public override bool Equals(object? other) =>
        other is Coord c
            && c.x.Equals(x)
            && c.y.Equals(y);

    // Implement IEquatable<T> https://stackoverflow.com/a/8952026/7532
    public bool Equals([AllowNull] Coord other) => x == other.x && y == other.y;

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public override string ToString() => $"({x},{y})";

    public IEnumerator<int> GetEnumerator()
    {
        yield return x;
        yield return y;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Coord RotateClockwise(int degrees)
    {
        return degrees switch
        {
            0 => new Coord(x, y),
            90 => new Coord(y, -x),
            180 => new Coord(-x, -y),
            270 => new Coord(-y, x),
            _ => throw new NotImplementedException()
        };
    }

    public int ManhattenDistanceTo(Coord other)
    {
        return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y);
    }
}
