namespace LogParser.Git;

public class AuthorChurnEntry(string author, int added, int deleted, int totalCommits)
{
    public string Author { get; } = author;
    public int Added { get; } = added;
    public int Deleted { get; } = deleted;
    public int TotalCommits { get; } = totalCommits;
}