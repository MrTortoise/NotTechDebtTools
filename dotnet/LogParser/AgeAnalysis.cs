using System.Text;

namespace LogParser;

public class AgeAnalysis(Dictionary<string, int> agedFiles)
{
    public Dictionary<string, int> AgedFiles { get; } = agedFiles;

    public static AgeAnalysis Analyse(List<Block> blocks, IGetToday getToday)
    {
        var agedFiles = new Dictionary<string, int>();
        foreach (var block in blocks)
        {
            var age = GetMonthDifference(block.OldestDate, getToday);
            foreach (var file in block.Files.Select(f=>f.FileName))
            {
                if (!agedFiles.TryAdd(file, age))
                {
                    if (agedFiles[file] < age)
                    {
                        agedFiles[file] = age;
                    }
                }
            }
        }
        return new AgeAnalysis(agedFiles);
    }
    
    private static int GetMonthDifference(DateTime startDate, IGetToday today)
    {
        DateTime endDate = today.Today;
        return Math.Abs((endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month);
    }

    public string ToCSV()
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