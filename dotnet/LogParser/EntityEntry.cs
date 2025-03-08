namespace LogParser;

public class EntityEntry(string fileName, int added, int deleted, int totalCommits)
{
    public string FileName { get; } = fileName;
    public int Added { get; } = added;
    public int Deleted { get; } = deleted;
    public int TotalCommits { get; } = totalCommits;
}