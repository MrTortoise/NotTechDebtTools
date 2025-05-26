using System.Text;

namespace LogParser.Git;

public class ActiveFileIdentificationAnalysis(Dictionary<string, int> agedFiles)
{
    public Dictionary<string, int> AgedFiles { get; } = agedFiles;

    public static ActiveFileIdentificationAnalysis Analyse(List<CommitBlock> blocks, IGetToday getToday)
    {
        var revisedFiles = new Dictionary<string, int>();
        foreach (var block in blocks)
        {
            var monthsSinceLastRevision = GetMonthDifference(block.DateOfLastRevision, getToday);
            foreach (var file in block.Files.Select(f=>f.FileName))
            {
                if (!revisedFiles.TryAdd(file, monthsSinceLastRevision))
                {
                    if (revisedFiles[file] > monthsSinceLastRevision)
                    {
                        revisedFiles[file] = monthsSinceLastRevision;
                    }
                }
            }
        }
        
        return new ActiveFileIdentificationAnalysis(revisedFiles);
    }
    
    private static int GetMonthDifference(DateTime startDate, IGetToday today)
    {
        DateTime endDate = today.Today;
        return Math.Abs((endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month);
    }
    
    public string ToCsv()
    {
        var sb = new StringBuilder();
        sb.AppendLine("entity,ageMonths");
        Dictionary<int,List<string>> byAge = AgedFiles.GroupBy(i=>i.Value).ToDictionary(i=>i.Key, i=>i.Select(j=>j.Key).ToList());
        foreach (var age in  byAge.Keys.Order())
        {
            foreach (var fileName in byAge[age])
            {
                sb.AppendLine($"{fileName},{age}");
            }
            
        }

        return sb.ToString();
    }
}