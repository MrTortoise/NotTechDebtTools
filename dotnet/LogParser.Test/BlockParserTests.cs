namespace LogParser.Test;

public class BlockParserTests
{
    private readonly BlockParser _blockParser = new();

    private const string ExampleData = """
                                       --eb637a20--2025-02-12--dependabot[bot]
                                       140	129	package-lock.json
                                       10	10	package.json

                                       --5335f9d6--2025-02-07--Dana Cotoran
                                       7	3	src/components/activity-history/index.njk
                                       0	1	src/components/activity-history/tests/activity-history-controller.test.ts
                                       2	2	src/locales/en/translation.json

                                       --364ed7a0--2025-02-11--Saral Kaushik
                                       --bfa61dd7--2025-02-11--Saral Kaushik
                                       6	0	package-lock.json
                                       1	0	package.json
                                       4	0	src/app.ts


                                       """;
    [Fact]
    public void Has3Blocks()
    {
        var blocks = BlockParser.GetBlocks(ExampleData);
        Assert.Equal(3, blocks.Count);
    }
    
    [Fact]
    public void SecondBlockHas3Files()
    {
        var blocks = BlockParser.GetBlocks(ExampleData);
        Assert.Equal(3, blocks[1].Files.Count);
    }
    
    [Fact]
    public void SecondBlockHasSecondFileName()
    {
        var blocks = BlockParser.GetBlocks(ExampleData);
        var fileName = blocks[1].Files[1].FileName;
        Assert.Equal("src/components/activity-history/tests/activity-history-controller.test.ts", fileName);
    }
}

