using System.Text.RegularExpressions;

namespace LogParser.Git;

/// <summary>
/// Has commits (commit entries) and files (file changes)
/// </summary>
public class CommitBlock
{
    /// <summary>
    /// In a block there are sometimes multiple commits, all except the last are related to merges
    /// </summary>
    public List<string> Mergers { get; private set; } = [];

    public DateTime DateOfLastRevision { get; private set; } = DateTime.MinValue;

    public List<CommitEntry> CommitEntries { get; } = [];
    public List<File> Files { get; } = [];

    /// <summary>
    /// A commit block has a single committer, in the log this is the lowest entry, the rest are mergers 
    /// </summary>
    public string Comitter { get; private set; } = string.Empty;
    

    
    public void Parse(string line, string ignoreMask)
    {
        var ignorePatterns = new List<string>();
        if (!string.IsNullOrWhiteSpace(ignoreMask))
        {
            ignorePatterns = ignoreMask.Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToList();
        }
        
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
            // format: "8	5	Dockerfile"
            var parts = line.Split("\t");
            var added = parts[0].Trim() == "-" ? 0 : Convert.ToInt32(parts[0]);
            var deleted = parts[1].Trim() == "-" ? 0 : Convert.ToInt32(parts[1]);
            var fileName = parts[2];
            if (!ignorePatterns.Any(p => MatchesWildcard(fileName, p)))
            {
                Files.Add(new File(added, deleted, fileName));
            }
        }
        
        Comitter = CommitEntries.Last().Committer;
        Mergers = CommitEntries.Take(CommitEntries.Count - 1).Select(i=>i.Committer).ToList();
    }
    
    /// <summary>
    /// Checks if a given text matches a wildcard pattern (where '*' matches any sequence of characters).
    /// The comparison is case-insensitive.
    /// </summary>
    /// <param name="text">The text to match (e.g., a file path).</param>
    /// <param name="pattern">The wildcard pattern (e.g., "*.csproj", "app.config").</param>
    /// <returns>True if the text matches the pattern, false otherwise.</returns>
    private static bool MatchesWildcard(string text, string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return false; // An empty pattern matches nothing
        }

        // Convert the wildcard pattern to a regular expression pattern.
        // Escape any regex special characters in the pattern, then replace '*' with '.*' (match zero or more of any character).
        // '^' and '$' ensure the pattern matches the entire string.
        var regexPattern = "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$";
        
        // Use Regex.IsMatch for case-insensitive matching.
        return Regex.IsMatch(text, regexPattern, RegexOptions.IgnoreCase);
    }
}

/// <summary>
/// Represents a single commit entry, typically used for the committer and subject of a commit.
/// </summary>
/// <param name="committer">The committer's name.</param>
/// <param name="subject">The commit subject/message.</param>
public class CommitEntry(string committer, string subject)
{
    /// <summary>
    /// Gets the committer's name.
    /// </summary>
    public string Committer { get; } = committer;

    /// <summary>
    /// Gets the commit subject.
    /// </summary>
    public string Subject { get; } = subject;
}