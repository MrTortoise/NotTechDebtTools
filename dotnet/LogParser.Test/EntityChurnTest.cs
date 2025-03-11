namespace LogParser.Test;

public class EntityChurnTest
{
    private const string ExampleData   = """
                                                    --440a3573--2025-01-27--Latif--Merge branch 'main' into OLH-2385-onboard-rp-department-for-education-apply-for-teacher-training
                                                    --7ab065f7--2025-01-23--Saral Kaushik--Merge pull request #1928 from govuk-one-login/OLH-2231-onboard-rp-home-office-standard-enhanced-applica
                                                    --9f3af8fc--2025-01-22--Saral Kaushik--add full stops
                                                    6	6	src/locales/cy/translation.json
                                                    6	6	src/locales/en/translation.json

                                                    --71098e4f--2025-01-22--Saral Kaushik--add welsh translation
                                                    36	0	src/locales/cy/translation.json

                                                    --17c42edb--2025-01-22--Saral Kaushik--add home office DBS check service
                                                    4	0	src/config.ts
                                                    36	0	src/locales/en/translation.json

                                                    --c723f6b6--2025-01-21--dependabot[bot]--Bump undici from 6.19.7 to 6.21.1
                                                    4	3	package-lock.json

                                                    --c06a35ba--2025-01-21--peterfajemisincabinetoffice--Merge pull request #1861 from govuk-one-login/OLH-2327
                                                    --4276f273--2025-01-21--Latif--Onboard Rp
                                                    4	0	src/config.ts
                                                    36	0	src/locales/en/translation.json

                                                    --a3dc7070--2025-01-21--Saral Kaushik--Merge pull request #1925 from govuk-one-login/OLH-2377-code-in-the-content-explaining-what-users-should-d
                                                    --d28d6acb--2025-01-21--Saral Kaushik--update tests
                                                    3	1	src/components/activity-history/tests/activity-history-controller.test.ts

                                                    --a7f70e92--2025-01-21--Saral Kaushik--Merge branch 'main' into OLH-2377-code-in-the-content-explaining-what-users-should-d
                                                    --1a9cc554--2025-01-21--Saral Kaushik--move translation into controller
                                                    1	1	localstack/provision.sh
                                                    9	0	src/components/activity-history/activity-history-controller.ts
                                                    3	3	src/components/activity-history/index.njk
                                                    5	7	src/locales/en/translation.json

                                                    """;
    
    private readonly List<CommitBlock> _blocks = BlockParser.GetBlocks(ExampleData);
    
    [Fact]
    public void Has8Entities()
    {
        var entities = EntityChurn.Analyse(_blocks);
        Assert.Equal(8, entities.ChurnedEntities.Count);
    }
    
    [Fact]
    public void EnTranslationHas8Commits()
    {
        var entities = EntityChurn.Analyse(_blocks);
        Assert.Equal(8, entities.ChurnedEntities["src/locales/en/translation.json"].TotalCommits);
    }
    
    [Fact]
    public void EnTranslationHas83Additions()
    {
        var entities = EntityChurn.Analyse(_blocks);
        Assert.Equal(83, entities.ChurnedEntities["src/locales/en/translation.json"].Added);
    }
    
    [Fact]
    public void EnTranslationHas13Deletions()
    {
        var entities = EntityChurn.Analyse(_blocks);
        Assert.Equal(13, entities.ChurnedEntities["src/locales/en/translation.json"].Deleted);
    }

    [Fact]
    public void ToCsvTest()
    {
        var entities = EntityChurn.Analyse(_blocks);
        var csv = entities.ToCsv();
        var expectedCsv = """
                          entity,added,deleted,commits
                          src/locales/en/translation.json,83,13,8
                          src/locales/cy/translation.json,42,6,4
                          src/config.ts,8,0,3
                          src/components/activity-history/tests/activity-history-controller.test.ts,3,1,2
                          localstack/provision.sh,1,1,2
                          src/components/activity-history/activity-history-controller.ts,9,0,2
                          src/components/activity-history/index.njk,3,3,2
                          package-lock.json,4,3,1
                          
                          """;
        Assert.Equal(expectedCsv, csv);
    }
}