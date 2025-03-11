using System.Text;

namespace LogParser;

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
        sb.AppendLine("entity,age-months");
        foreach (var age in AgedFiles)
        {
            sb.AppendLine($"{age.Key},{age.Value}");
        }

        return sb.ToString();
    }
}