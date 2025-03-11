using System.Text;

namespace LogParser;

public class HotSpotAnalysis(Dictionary<string, HotSpot> hotSpots)
{
    public Dictionary<string, HotSpot> HotSpots { get; } = hotSpots;

    public static HotSpotAnalysis Analyse(List<Block> blocks)
    {
        var entities = new Dictionary<string, HotSpot>(); 
        foreach (var block in blocks)
        {
            var revisions = block.Committers.Count;
            foreach (var file in block.Files)
            {
                var fileName = file.FileName;
                if (!entities.ContainsKey(fileName))
                {
                    entities[fileName] = new HotSpot(fileName);
                }

                var existing = entities[fileName];
                entities[fileName] = existing.Update(block.Committers.Select(c => c.Committer), revisions);
            }
        }
        
        return new HotSpotAnalysis(entities);
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