using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public class Day7 : ISolver
{
    public (string, string) ExpectedResult => ("1778099", "");

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
        public Folder Parent { get; }

        public Folder(string name, Folder parent)
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
        Folder currentFolder = null;
        foreach (var line in input)
        {
            if (line.StartsWith("$ cd"))
            {
                if (line.EndsWith(".."))
                {
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
                currentFolder.Files.Add(new File(match.Groups[2].Value, long.Parse(match.Groups[1].Value)));
            }
        }
        var root = currentFolder;
        while(root.Parent != null) { root = currentFolder.Parent; }
        return root;
    }



    public long Part1(IEnumerable<string> input)
    {
        var folder = ParseInput(input);
        return folder.GetChildFolders().Select(v =>v.GetSize()).Where(v => v <= 100000).Sum();
    }
    long Part2(IEnumerable<string> input) => 0;

}
