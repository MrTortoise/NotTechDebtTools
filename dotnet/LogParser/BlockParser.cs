namespace LogParser;

public class BlockParser
{
    private List<Block> GetBlocks(string input)
    {
        input = input.Trim();
        var blocks = new List<Block>();
        var block = new Block();
        foreach (var line in input.Split('\n'))
        {
            // end of block
            if (string.IsNullOrWhiteSpace(line))
            {
                if (block.Authors.Count > 0)
                {
                    blocks.Add(block);
                }
                
                block = new Block();
                continue;
            }
            
            block.Parse(line);
        }
        if (block.Authors.Count > 0)
        {
            blocks.Add(block);
        }
        
        return blocks;
    }
}