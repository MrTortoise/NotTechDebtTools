using System.Text;

namespace LogParser.Git;

public class AuthorChurn(Dictionary<string, AuthorChurnEntry> authorChurnEntries)
{
    public Dictionary<string, AuthorChurnEntry> AuthorChurnEntries { get; } = authorChurnEntries;

    public static AuthorChurn Analyse(List<CommitBlock> blocks)
    {
        var authors = new Dictionary<string, AuthorChurnEntry>();
        foreach (var block in blocks)
        {
            var added = block.Files.Sum(f => f.LinesAdded);
            var deleted = block.Files.Sum(f => f.LinesDeleted);
            var committer = block.Comitter;
            if (!authors.ContainsKey(committer))
            {
                authors[committer] = new AuthorChurnEntry(committer, added, deleted, 1);
            }
            else
            {
                var existing = authors[committer];
                authors[committer] = new AuthorChurnEntry(
                    committer,
                    existing.Added + added,
                    existing.Deleted + deleted,
                    existing.TotalCommits + 1);
            }
        }

        return new AuthorChurn(authors);
    }

    public string ToCsv()
    {
        var sb = new StringBuilder();
        sb.AppendLine("author,added,deleted,commits");
        foreach (var entry in AuthorChurnEntries.Values.OrderByDescending(i => i.Added + i.Deleted))
        {
            sb.AppendLine($"{entry.Author},{entry.Added},{entry.Deleted},{entry.TotalCommits}");
        }

        return sb.ToString();
    }
}