using System.Text;

namespace AdventOfCode2022;

public class Grid<T>
{
    // for those cases where the grid is single digit numbers
    public static Grid<T> ParseToGrid(string[] input, Func<char,T> selector)
    {
        var grid = new Grid<T>(input[0].Length, input.Length);
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
                grid[(x, y)] = selector(input[y][x]);
        }
        return grid;
    }

    private T[,] items;

    public Grid(int x, int y)
    {
        items = new T[x, y];
        Height = y;
        Width = x;  
    }

    public Grid(int x, int y, T defaultValue)
        :this(x,y)
    {
        foreach(var p in AllPositions())
            this[p] = defaultValue;
    }


    public T this[Coord c]
    {
        get { return items[c.X,c.Y]; }
        set { items[c.X, c.Y] = value; }
    }

    public int Width { get; set;  }
    public int Height { get; set; }

    public bool IsInGrid(Coord point) => point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;

    public IEnumerable<Coord> AllPositions()
    {
        for(int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                yield return new Coord(x, y);
    }

    public IEnumerable<Coord> LineOut(Coord startingPos, Coord delta)
    {
        var pos = startingPos + delta;
        while(IsInGrid(pos))
        {
            yield return pos;
            pos += delta;
        }
    }

    public IEnumerable<Coord> Neighbours(Coord p, bool includeDiagonals = false)
    {
        return p.Neighbours(includeDiagonals).Select(n => p + n).Where(IsInGrid);
    }

    public override string ToString()
    {
        var s = new StringBuilder();
        for(var y = 0;y<Height;y++)
        {
            for(var x= 0; x < Width;x++)
            {
                s.Append($"{this[(x, y)]}");
            }
            s.AppendLine();
        }
        return s.ToString();
    }
}
