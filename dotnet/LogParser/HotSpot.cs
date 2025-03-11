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
    
    public HotSpot AddAuthor(string author)
    {
        var newAuthors = new HashSet<string>(authors) { author };
        return new HotSpot(
            FileName, 
            newAuthors,
            NumberOfRevisions+1);
    }
}