using System.Text;

namespace LogParser.Git;

public class EntityChurn(Dictionary<string, EntityEntry> churnedEntities)
{
    public Dictionary<string, EntityEntry> ChurnedEntities { get; } = churnedEntities;

    public static EntityChurn Analyse(List<CommitBlock> blocks)
    {
        var churnedEntities = new Dictionary<string, EntityEntry>();
        foreach (var block in blocks)
        {
            var commits = block.CommitEntries.Count;
            foreach (var file in block.Files)
            {
                if (!churnedEntities.ContainsKey(file.FileName))
                {
                    churnedEntities.Add(file.FileName, new EntityEntry(file.FileName,0,0,0));
                }
                
                var existing = churnedEntities[file.FileName];
                churnedEntities[file.FileName] = new EntityEntry(
                    file.FileName, 
                    file.LinesAdded + existing.Added,
                    file.LinesDeleted + existing.Deleted, 
                    existing.TotalCommits + commits);
            }
        }
        return new EntityChurn(churnedEntities);
    }

    public string ToCsv()
    {
        var sb = new StringBuilder();
        sb.AppendLine("entity,added,deleted,commits");
        foreach (var entry in ChurnedEntities.Values.OrderByDescending(i=>i.TotalCommits))
        {
            sb.AppendLine($"{entry.FileName},{entry.Added},{entry.Deleted},{entry.TotalCommits}");
        }

        return sb.ToString();
    }
}