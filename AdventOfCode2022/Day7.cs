using System.Text.RegularExpressions;
using static AdventOfCode2022.Day7;

namespace AdventOfCode2022;

public class Day7 : ISolver
{
    public (string, string) ExpectedResult => ("1778099", "1623571");

    public (string, string) Solve(string[] input)
    {
        return ($"{Part1(input)}", $"{Part2(input)}");
    }

    public record File(string Name, long Size);
    public class Folder
    {
        public List<Folder> Folders { get; }
        public List<File> Files { get; }
        public string Name { get; }
        public Folder? Parent { get; }

        public Folder(string name, Folder? parent)
        {
            Files = new List<File>();
            Folders = new List<Folder>();
            Name = name;
            Parent = parent;
        }

        public long GetSize()
        {
            return Files.Sum(f => f.Size)
                   + Folders.Sum(f => f.GetSize());
        }

        public IEnumerable<Folder> GetChildFolders()
        {
            foreach(var f in Folders)
            {
                yield return f;
                foreach(var child in f.GetChildFolders())
                {
                    yield return child;
                }
            }
        }
    }

    public Folder ParseInput(IEnumerable<string> input)
    {
        Folder? currentFolder = null;
        foreach (var line in input)
        {
            if (line.StartsWith("$ cd"))
            {
                if (line.EndsWith(".."))
                {
                    if (currentFolder == null) throw new InvalidOperationException("no current folder, can't go up");
                    currentFolder = currentFolder.Parent;
                }
                else
                {
                    var f = new Folder(line[5..], currentFolder);
                    currentFolder?.Folders.Add(f);
                    currentFolder = f;
                }
            }
            else if (line.StartsWith("$ ls"))
            {

            }
            else if (line.StartsWith("dir"))
            {

            }
            else
            {
                var regex = new Regex(@"(\d+) (\S+)");
                var match = regex.Match(line);
                if (currentFolder == null) throw new InvalidOperationException("no current folder, can't add file");
                currentFolder.Files.Add(new File(match.Groups[2].Value, long.Parse(match.Groups[1].Value)));
            }
        }
        if (currentFolder == null) throw new InvalidOperationException("no folders found in input");
        var root = currentFolder!;
        while(root!.Parent != null) { root = root.Parent; }
        return root;
    }



    public long Part1(IEnumerable<string> input)
    {
        var folder = ParseInput(input);
        return folder.GetChildFolders().Select(v =>v.GetSize()).Where(v => v <= 100000).Sum();
    }

    public long Part2(IEnumerable<string> input)
    {
        var diskSize = 70000000L;
        var needed = 30000000L;
        var rootFolder = ParseInput(input);
        var available = diskSize - rootFolder.GetSize();
        var toDelete = needed - available;
        return rootFolder.GetChildFolders().Select(v => v.GetSize()).OrderBy(n => n).Where(v => v >= toDelete).First();
    }

}
