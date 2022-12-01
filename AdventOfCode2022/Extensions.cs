namespace AdventOfCode2022;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> GroupsWithSeparator<T>(this IEnumerable<T> source, Func<T, bool> separator)
    {
        var group = new List<T>();
        foreach(var item in source)
        {
            if (separator(item))
            {
                yield return group;
                group = new List<T>();
            }
            else
            {
                group.Add(item);
            }
        }
        yield return group;
    }
}