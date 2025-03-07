namespace LogParser;

internal class Block
{
    public void Parse(string line)
    {
        // author line
        if (line.StartsWith("--"))
        {
            var parts = line.Split("--");
            var date = DateTime.Parse(parts[2]);
            var author = parts[3];
            
            Authors.Add(author);
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

    public List<string> Authors { get; } = [];
    public List<File> Files { get; } = [];
}