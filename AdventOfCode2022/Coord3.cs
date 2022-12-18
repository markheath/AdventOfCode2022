using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022;

public readonly struct Coord3 : IEnumerable<int>, IEquatable<Coord3>
{
    private readonly int x;
    private readonly int y;
    private readonly int z;
    public int X { get => x; }
    public int Y { get => y; }
    public int Z { get => z; }

    public int this[int index]
    {
        get { return index == 0 ? x : y; }
    }

    public Coord3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static implicit operator (int, int, int)(Coord3 c) => (c.x, c.y, c.z);
    public static implicit operator Coord3((int X, int Y, int Z) c) => new(c.X, c.Y, c.Z);

    public void Deconstruct(out int x, out int y)
    {
        x = this.x;
        y = this.y;
    }

    public static Coord3 operator +(Coord3 a, Coord3 b)
    {
        return new Coord3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public override bool Equals(object? obj)
    {
        return obj is Coord3 c
            && c.x.Equals(x)
            && c.y.Equals(y)
            && c.z.Equals(z);
    }

    // Implement IEquatable<T> https://stackoverflow.com/a/8952026/7532
    public bool Equals([AllowNull] Coord3 other) => x == other.x && y == other.y && z == other.z;


    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    public override string ToString() => $"({x},{y},{z})";

    public IEnumerator<int> GetEnumerator()
    {
        yield return x;
        yield return y;
        yield return z;
    }

    private static readonly IEnumerable<Coord3> horizontalNeighbours = new Coord3[] 
    { (-1, 0, 0), (1, 0, 0), (0,1,0), (0,-1,0), (0,0,-1), (0,0,1) };
    /*private static readonly IEnumerable<Coord3> diagonalNeigbours = new Coord3[] 
    { new (-1, -1), new (1, 1), new (1, -1), new (-1, 1) };*/

    public IEnumerable<Coord3> Neighbours(bool includeDiagonals = false)
    {
        //var neighbours = includeDiagonals ? horizontalNeighbours.Concat(diagonalNeigbours) : horizontalNeighbours;
        var neighbours = horizontalNeighbours;
        var p = this;
        return neighbours.Select(n => p + n);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Coord3 RotateClockwiseAroundZ(int degrees)
    {
        return degrees switch
        {
            0 => this,
            90 => new Coord3(y, -x, z),
            180 => new Coord3(-x, -y, z),
            270 => new Coord3(-y, x, z),
            _ => throw new NotImplementedException()
        };
    }

    public static bool operator ==(Coord3 left, Coord3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Coord3 left, Coord3 right)
    {
        return !(left == right);
    }
}