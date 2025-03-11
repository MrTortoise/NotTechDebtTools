using System.Text;

namespace LogParser;

public class ActivityHotSpotAnalysis(Dictionary<string, HotSpot> hotSpots)
{
    public Dictionary<string, HotSpot> HotSpots { get; } = hotSpots;

    public static ActivityHotSpotAnalysis Analyse(List<CommitBlock> blocks)
    {
        var entities = new Dictionary<string, HotSpot>(); 
        foreach (var block in blocks)
        {
            foreach (var file in block.Files)
            {
                var fileName = file.FileName;
                if (!entities.ContainsKey(fileName))
                {
                    entities[fileName] = new HotSpot(fileName);
                }

                var existing = entities[fileName];
                entities[fileName] = existing.AddAuthor(block.Comitter);
            }
        }
        
        return new ActivityHotSpotAnalysis(entities);
    }

    public string ToCsv()
    {
        var sb = new StringBuilder();
        sb.AppendLine("entity,numberOfAuthors,numberOfRevisions");
        foreach (var entry in HotSpots.Values.OrderByDescending(i=>i.NumberOfAuthors+i.NumberOfRevisions))
        {
            sb.AppendLine($"{entry.FileName},{entry.NumberOfAuthors},{entry.NumberOfRevisions}");
        }

        return sb.ToString();
    }
}