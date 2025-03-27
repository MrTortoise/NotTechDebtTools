using LogParser.Git;

namespace LogParser.Test.Git
{
    public class BlockParserTests
    {
        private readonly BlockParser _blockParser = new();

        private const string ExampleData = """
                                           --eb637a20--2025-02-12--dependabot[bot]--BAU: Bump the dev-dependencies group across 1 directory with 10 updates
                                           140	129	package-lock.json
                                           10	10	package.json

                                           --5335f9d6--2025-02-07--Dana Cotoran--FIX content problems on activity log
                                           7	3	src/components/activity-history/index.njk
                                           0	1	src/components/activity-history/tests/activity-history-controller.test.ts
                                           2	2	src/locales/en/translation.json

                                           --364ed7a0--2025-02-11--Saral Kaushik--Merge branch 'main' into OLH-2414-update-frontend-to-use-centralized-library
                                           --bfa61dd7--2025-02-11--Saral Kaushik--import library
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

        [Fact]
        public void CanParseNonTextBlock()
        {
            var example= """
                         --6348335c--2022-10-27--Huw Diprose--GUA-477: ADR decide how we will read data out of account data store
                         61	0	docs/adr/0003-read-data-out-of-account-store-into-app.md
                         -	-	docs/adr/images/2022-reading-data-out-adopted.png
                         -	-	docs/adr/images/2022-reading-data-out-rejected-api-gateway.png
                         -	-	docs/adr/images/2022-reading-data-out-rejected-lambda.png
                         """;
            var blocks = BlockParser.GetBlocks(example);
            Assert.Equal(4, blocks[0].Files.Count);
        
        }
    }
}

