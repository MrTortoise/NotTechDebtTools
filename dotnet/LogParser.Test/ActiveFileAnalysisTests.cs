namespace LogParser.Test;

public class ActiveFileAnalysisTests
{
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

    private readonly List<Block> _blocks = BlockParser.GetBlocks(ExampleData);
    
    [Fact]
    public void Has6Files()
    {
        var ages = ActiveFileIdentificationAnalysis.Analyse(_blocks, new MockToday());
        Assert.Equal(6, ages.AgedFiles.Count);
    }
    
    [Fact]
    public void AgeOfTranslation()
    {
        var expectedDate = new DateTime(2025, 02, 07);
        var years = (DateTime.Now.Year - expectedDate.Year)*12;
        var months = DateTime.Now.Month - expectedDate.Month;
        var total = years + months;
        var ages = ActiveFileIdentificationAnalysis.Analyse(_blocks, new MockToday());
        Assert.Equal(total, ages.AgedFiles["src/locales/en/translation.json"]);
    }

    [Fact]
    public void ToCsv()
    {
        var ages = ActiveFileIdentificationAnalysis.Analyse(_blocks, new MockToday());
        var result = ages.ToCsv();
        var expected = """
                       entity,age-months
                       package-lock.json,1
                       package.json,1
                       src/components/activity-history/index.njk,1
                       src/components/activity-history/tests/activity-history-controller.test.ts,1
                       src/locales/en/translation.json,1
                       src/app.ts,1
                       
                       """;
        Assert.Equal(expected,result);
    }

}

public class MockToday : IGetToday
{
    public DateTime Today => new(2025, 03, 07);
}