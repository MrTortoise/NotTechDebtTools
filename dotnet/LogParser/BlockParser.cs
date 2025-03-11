namespace LogParser;

public class BlockParser
{
    public static List<CommitBlock> GetBlocks(string input)
    {
        input = input.Trim();
        var blocks = new List<CommitBlock>();
        var block = new CommitBlock();
        foreach (var line in input.Split('\n'))
        {
            // end of block
            if (string.IsNullOrWhiteSpace(line))
            {
                if (block.CommitEntries.Count > 0)
                {
                    blocks.Add(block);
                }
                
                block = new CommitBlock();
                continue;
            }
            
            block.Parse(line);
        }
        if (block.CommitEntries.Count > 0)
        {
            blocks.Add(block);
        }
        
        return blocks;
    }
}