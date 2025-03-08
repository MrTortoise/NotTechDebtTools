using System.Text;

namespace LogParser;

public class AuthorChurn(Dictionary<string, AuthorChurnEntry> authorChurnEntries)
{
    public Dictionary<string, AuthorChurnEntry> AuthorChurnEntries { get; } = authorChurnEntries;

    public static AuthorChurn Analyse(List<Block> blocks)
    {
        var authors = new Dictionary<string, AuthorChurnEntry>();
        foreach (var block in blocks)
        {
            var added = block.Files.Sum(f => f.LinesAdded);
            var deleted = block.Files.Sum(f => f.LinesDeleted);

            var committersSeenThisBlock = new HashSet<string>();
            foreach (var committer in block.Committers.Select(c=>c.Committer))
            {
                if (!authors.ContainsKey(committer))
                {
                    authors[committer] = new AuthorChurnEntry(committer, added, deleted, 1);
                }
                else
                {
                    var existing = authors[committer];
                    if (committersSeenThisBlock.Contains(committer))
                    {
                        // We dont want to double count adds when multiple commits by same person in a block
                        authors[committer] = new AuthorChurnEntry(
                            committer, 
                            existing.Added, 
                            existing.Deleted,
                            existing.TotalCommits + 1);
                    }
                    else
                    {
                        authors[committer] = new AuthorChurnEntry(
                            committer, 
                            existing.Added + added, 
                            existing.Deleted + deleted,
                            existing.TotalCommits + 1);
                    }
                }
                committersSeenThisBlock.Add(committer);
            }
        }

        return new AuthorChurn(authors);
    }

    public string ToCsv()
    {
        var sb = new StringBuilder();
        sb.AppendLine("author,added,deleted,commits");
        foreach (var entry in AuthorChurnEntries)
        {
            sb.AppendLine($"{entry.Key},{entry.Value.Added},{entry.Value.Deleted},{entry.Value.TotalCommits}");
        }

        return sb.ToString();
    }
}