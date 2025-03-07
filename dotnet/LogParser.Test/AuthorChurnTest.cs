namespace LogParser.Test;

public class AuthorChurnTest
{
    private const string ExampleData = """
                                       --6262d303--2025-02-10--Latif
                                       --55f31920--2025-02-06--danacotoran
                                       --57a5bfc9--2025-02-06--Latif
                                       --3a511c4b--2025-02-06--Latif
                                       --6d65223c--2025-01-31--Dana Cotoran
                                       1	2	src/components/activity-history/activity-history-controller.ts
                                       1	1	src/components/report-suspicious-activity/report-suspicious-activity-controller.ts
                                       1	1	src/components/your-services/your-services-controller.ts
                                       5	7	src/utils/activityHistory.ts
                                       3	3	src/utils/errors.ts
                                       1	1	src/utils/http.ts
                                       1	1	src/utils/mfa/index.ts
                                       2	4	src/utils/redact.ts
                                       1	3	src/utils/referenceCode.ts
                                       2	5	src/utils/types.ts

                                       --6e99c581--2025-02-03--Latif
                                       1	1	src/app.ts

                                       --0f56a9ee--2025-02-03--Latif
                                       356	138	package-lock.json
                                       1	1	package.json

                                       --57a1ef24--2025-02-03--Latif
                                       7	7	src/locales/cy/translation.json
                                       7	7	src/locales/en/translation.json

                                       --7a1dfdbc--2025-01-31--di-aholme
                                       --ea0f575b--2025-01-31--peterfajemisincabinetoffice
                                       --8d0b48f8--2025-01-30--di-aholme
                                       0	3	.env.sample
                                       0	27	deploy/template.yaml
                                       2	4	package.json
                                       0	46	src/assets/javascript/application.js
                                       0	154	src/assets/javascript/cookies.js
                                       0	30	src/assets/javascript/tests/cookies.test.js
                                       1	3	src/components/common/layout/base.njk
                                       0	8	src/config.ts
                                       0	4	src/middleware/set-local-vars-middleware.ts
                                       """;
    
    private readonly List<Block> _blocks = BlockParser.GetBlocks(ExampleData);
    
    [Fact]
    public void Has5Authors()
    {
        var authors = AuthorChurn.Analyse(_blocks);
        Assert.Equal(5, authors.AuthorChurnEntries.Count);
    }

    [Fact]
    public void LatiffHas6Commits()
    {
        var authors = AuthorChurn.Analyse(_blocks);
        Assert.Equal(6, authors.AuthorChurnEntries["Latif"].TotalCommits);
    }
    
    [Fact]
    public void LatiffHas390Adds()
    {
        var authors = AuthorChurn.Analyse(_blocks);
        Assert.Equal(390, authors.AuthorChurnEntries["Latif"].Added);
    }
    
    [Fact]
    public void LatiffHas182Deletes()
    {
        var authors = AuthorChurn.Analyse(_blocks);
        Assert.Equal(182, authors.AuthorChurnEntries["Latif"].Deleted);
    }
    
    [Fact]
    public void ToCsv()
    {
        var authors = AuthorChurn.Analyse(_blocks);
        var result = authors.ToCsv();
        var expected = """
                       author,added,deleted,commits
                       Latif,390,182,6
                       danacotoran,18,28,1
                       Dana Cotoran,18,28,1
                       di-aholme,3,279,2
                       peterfajemisincabinetoffice,3,279,1

                       """;
        Assert.Equal(expected,result);
    }
}