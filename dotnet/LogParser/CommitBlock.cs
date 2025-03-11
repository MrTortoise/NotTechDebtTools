namespace LogParser;

public class Block
{
    public void Parse(string line)
    {
        // author line
        if (line.StartsWith("--"))
        {
            var parts = line.Split("--");
            var date = DateTime.Parse(parts[2]);
            var committer = parts[3];
            var subject = parts[4];
            
            CommitEntries.Add(new CommitEntry(committer, subject));
            if (DateOfLastRevision < date)
            {
                DateOfLastRevision = date;
            }
        }
        else
        {
            var parts = line.Split("\t");
            var added = parts[0].Trim() == "-" ? 0 : Convert.ToInt32(parts[0]);
            var deleted = parts[1].Trim() == "-" ? 0 : Convert.ToInt32(parts[1]);
            Files.Add(new File(added, deleted, parts[2]));
        }
        
        Comitter = CommitEntries.Last().Committer;
        Mergers = CommitEntries.Take(CommitEntries.Count - 1).ToList();
    }

    public List<CommitEntry> Mergers { get; private set; }

    public DateTime DateOfLastRevision { get; private set; } = DateTime.MinValue;

    public List<CommitEntry> CommitEntries { get; } = [];
    public List<File> Files { get; } = [];
    
    public string Comitter { get; private set; }
    
}

public class CommitEntry(string committer, string subject)
{
    public string Committer { get; } = committer;
    public string Subject { get; } = subject;
}