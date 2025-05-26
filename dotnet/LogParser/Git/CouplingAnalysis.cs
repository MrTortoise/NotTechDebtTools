using System.Text;

namespace LogParser.Git;

public class CouplingAnalysis(Dictionary<string, FileCouplings> couplingSources)
{
    public Dictionary<string, FileCouplings> CouplingSources { get; } = couplingSources;

    public static CouplingAnalysis Analyse(List<CommitBlock> blocks)
    {
        var coupledFiles = new Dictionary<string, FileCouplings>();
        foreach (var block in blocks)
        {
            foreach (var file in block.Files)
            {
                //for each file in each block add a coupling entry to it from every other file in the block
                var fileName = file.FileName;
                if (!coupledFiles.ContainsKey(fileName))
                {
                    coupledFiles[fileName] = new FileCouplings(fileName);
                }
                
                var entry = coupledFiles[fileName];
                entry.AddCouplingTo(block.Files.Select(f=>f.FileName));
            }
        }
        
        return new CouplingAnalysis(coupledFiles);
    }

    public string ToCsv(int minimumFrequency, int minimumProbability)
    {
        var sb = new StringBuilder();
        sb.AppendLine("source,target,frequency,probability");
        var reverse = new HashSet<string>();
        foreach (var couplingData in CouplingSources.Values
                     .SelectMany(couplingSource => couplingSource)
                     .OrderByDescending(c=>c.Probability)
                     .Where(i=>i.Probability >= minimumProbability)
                     .Where(i=>i.Frequency >= minimumFrequency))
        {
            if (reverse.Contains($"{couplingData.Target},{couplingData.Source}")) continue;
            
            sb.AppendLine($"{couplingData.Source},{couplingData.Target},{couplingData.Frequency},{couplingData.Probability}");
            reverse.Add($"{couplingData.Source},{couplingData.Target}");
        }

        return sb.ToString();
    }


}