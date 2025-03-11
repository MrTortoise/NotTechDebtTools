namespace LogParser;

public class HotSpot(string fileName, IEnumerable<string> authors, int numberOfRevisions)
{
    public HotSpot(string fileName)
        :this(fileName, [], 0)
    {
      
    }

    public string FileName { get; } = fileName;
    private HashSet<string> Authors { get; } = [..authors];
    public int NumberOfAuthors => Authors.Count;
    public int NumberOfRevisions { get; } = numberOfRevisions;
    
    public HotSpot Update(IEnumerable<string> authors, int revisions)
    {
        var newAuthors = new HashSet<string>(authors);
        newAuthors.UnionWith(Authors);
        return new HotSpot(
            FileName, 
            newAuthors,
            NumberOfRevisions + revisions);
    }
}