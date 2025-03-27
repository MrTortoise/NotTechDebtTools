using LogParser.Git;

namespace LogParser.Test.Git
{
    public class ActivityHotspotAnalysisTest
    {
        private const string ExampleData = """
                                           --5335f9d6--2025-02-07--Dana Cotoran--FIX content problems on activity log
                                           7	3	src/components/activity-history/index.njk
                                           0	1	src/components/activity-history/tests/activity-history-controller.test.ts
                                           2	2	src/locales/en/translation.json

                                           --364ed7a0--2025-02-11--Saral Kaushik--Merge branch 'main' into OLH-2414-update-frontend-to-use-centralized-library
                                           --bfa61dd7--2025-02-11--Saral Kaushik--import library
                                           6	0	package-lock.json
                                           1	0	package.json
                                           4	0	src/app.ts

                                           --52d93e0f--2025-02-11--Peter Fajemisin--OLH-2378 - Update the service card: HO - IAA Home Service Card
                                           28	28	src/locales/cy/translation.json
                                           21	21	src/locales/en/translation.json

                                           --4da3a7a7--2025-02-11--dependabot[bot]--BAU: Bump the production-dependencies group across 1 directory with 11 updates
                                           644	617	package-lock.json
                                           10	10	package.json

                                           --0292d774--2025-02-11--Latif--Merge pull request #1954 from govuk-one-login/OLH-2317-update-to-express-version-5
                                           --a760ec33--2025-02-11--dependabot[bot]--BAU: Bump node from 20.18.1-alpine to 20.18.3-alpine
                                           2	2	Dockerfile
                                           1	1	local.Dockerfile

                                           --24c52b10--2025-02-11--dependabot[bot]--BAU: Bump node in /post-deploy-tests
                                           1	1	post-deploy-tests/Dockerfile

                                           --9e31f7f3--2025-02-11--dependabot[bot]--BAU: Bump the dev-dependencies group across 1 directory with 10 updates
                                           140	129	package-lock.json
                                           10	10	package.json
                                           """;
    
        private readonly List<CommitBlock> _blocks = BlockParser.GetBlocks(ExampleData);
    
        [Fact]
        public void Has10Entities()
        {
            var hotSpotAnalysis = ActivityHotSpotAnalysis.Analyse(_blocks);
            Assert.Equal(10, hotSpotAnalysis.HotSpots.Count);
        }
    
        [Fact]
        public void PackageHas2Authors()
        {
            var file = "package.json";
            var hotSpotAnalysis = ActivityHotSpotAnalysis.Analyse(_blocks);
            Assert.Equal(2, hotSpotAnalysis.HotSpots[file].NumberOfAuthors);
        }
    
        [Fact]
        public void PackageHas3Revisions()
        {
            var file = "package.json";
            var hotSpotAnalysis = ActivityHotSpotAnalysis.Analyse(_blocks);
            Assert.Equal(3, hotSpotAnalysis.HotSpots[file].NumberOfRevisions);
        }
    
        [Fact]
        public void ToCsvTest()
        {
            var hotSpotAnalysis = ActivityHotSpotAnalysis.Analyse(_blocks);
            var csv = hotSpotAnalysis.ToCsv();
            var expectedCsv = """ 
                              entity,numberOfAuthors,numberOfRevisions
                              package-lock.json,2,3
                              package.json,2,3
                              src/locales/en/translation.json,2,2
                              src/components/activity-history/index.njk,1,1
                              src/components/activity-history/tests/activity-history-controller.test.ts,1,1
                              src/app.ts,1,1
                              src/locales/cy/translation.json,1,1
                              Dockerfile,1,1
                              local.Dockerfile,1,1
                              post-deploy-tests/Dockerfile,1,1

                              """;
            Assert.Equal(expectedCsv, csv);
        }
    }
}