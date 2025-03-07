namespace LogParser;

public class Block
{
    public void Parse(string line)
    {
        // author line
        if (line.StartsWith("--"))
        {
            var parts = line.Split("--");
            var date = DateTime.Parse(parts[2]);
            var committer = parts[3];
            
            Committers.Add(committer);
            if (OldestDate > date)
            {
                OldestDate = date;
            }
        }
        else
        {
            var parts = line.Split("\t");
            Files.Add(new File(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), parts[2]));
        }
    }

    public DateTime OldestDate { get; private set; } = DateTime.Today;

    public List<string> Committers { get; } = [];
    public List<File> Files { get; } = [];
}