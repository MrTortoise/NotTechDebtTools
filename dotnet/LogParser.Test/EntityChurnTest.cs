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
    
    private readonly List<Block> _blocks = BlockParser.GetBlocks(ExampleData);
    
    [Fact]
    public void Has8Entities()
    {
        var entitys = EntityChurn.Analyse(_blocks);
        Assert.Equal(8, entitys.ChurnedEntities.Count);
    }
    
    [Fact]
    public void EnTranslationHas8Commits()
    {
        var entitys = EntityChurn.Analyse(_blocks);
        Assert.Equal(8, entitys.ChurnedEntities["src/locales/en/translation.json"].Commits);
    }
    
    [Fact]
    public void EnTranslationHas83Additions()
    {
        var entitys = EntityChurn.Analyse(_blocks);
        Assert.Equal(83, entitys.ChurnedEntities["src/locales/en/translation.json"].Added);
    }
    
    [Fact]
    public void EnTranslationHas13Delettions()
    {
        var entitys = EntityChurn.Analyse(_blocks);
        Assert.Equal(13, entitys.ChurnedEntities["src/locales/en/translation.json"].Deleted);
    }
}

public class EntityChurn(Dictionary<string, EntityEntry> churnedEntities)
{
    public Dictionary<string, EntityEntry> ChurnedEntities { get; } = churnedEntities;

    public static EntityChurn Analyse(List<Block> blocks)
    {
        var churnedEntities = new Dictionary<string, EntityEntry>();
        foreach (var block in blocks)
        {
            var commits = block.Committers.Count;
            foreach (var file in block.Files)
            {
                if (!churnedEntities.ContainsKey(file.FileName))
                {
                    churnedEntities.Add(file.FileName, new EntityEntry(file.FileName,0,0,0));
                }
                
                var existing = churnedEntities[file.FileName];
                churnedEntities[file.FileName] = new EntityEntry(
                    file.FileName, 
                    file.LinesAdded + existing.Added,
                    file.LinesDeleted + existing.Deleted, 
                    existing.Commits + commits);
            }
        }
        return new EntityChurn(churnedEntities);
    }
}

public class EntityEntry(string fileName, int added, int deleted, int commits)
{
    public string FileName { get; } = fileName;
    public int Added { get; } = added;
    public int Deleted { get; } = deleted;
    public int Commits { get; } = commits;
}