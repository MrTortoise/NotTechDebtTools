using LogParser.Git;

namespace LogParser.Test.Git
{
    public class HistogramTest
    {
        private const string ExampleData = """
                                           --43bd839--2025-02-07--DAVE\McInnesA--s
                                           6	21	Dockerfile
                                           
                                           --535cef7--2025-02-06--DAVE\McInnesA--d
                                           9	0	Dockerfile
                                           1	1	docker-compose.yml
                                           
                                           --48b4752--2025-02-06--DAVE\McInnesA--fdc
                                           7	14	Dockerfile
                                           
                                           --fb98c90--2025-02-06--DAVE\McInnesA--t5
                                           16	14	Dockerfile
                                           1	1	Makefile
                                           1	1	docker-compose.yml
                                           1	1	boopnet-alerts
                                           
                                           --35828c8--2025-02-06--DAVE\McInnesA--x
                                           15	8	Dockerfile
                                           
                                           --16b8b53--2025-02-06--DAVE\McInnesA--different install method
                                           10	5	Dockerfile
                                           
                                           --a0c734c--2025-02-06--DAVE\McInnesA--update docker to have net8 and set connection string env var for dockercompose
                                           6	0	Dockerfile
                                           3	2	Makefile
                                           23	8	docker-compose.yml
                                           
                                           --9a8d5d5--2025-01-24--andyr-DAVE--KPR-698: Updated pipeline to use stages
                                           1	1	Dockerfile
                                           274	284	azure-pipelines.yml
                                           """;
    
        private readonly List<CommitBlock> _blocks = BlockParser.GetBlocks(ExampleData);
    
        // [Fact]
        // public void Has10Entities()
        // {
        //     var histogramAnalysis = ChangeHistogram.Analyse(_blocks);
        //     Assert.Equal(10, histogramAnalysis.Months.Count);
        // }
    
//         [Fact]
//         public void PackageHas2Authors()
//         {
//             var file = "package.json";
//             var hotSpotAnalysis = ActivityHotSpotAnalysis.Analyse(_blocks);
//             Assert.Equal(2, hotSpotAnalysis.HotSpots[file].NumberOfAuthors);
//         }
//     
//         [Fact]
//         public void PackageHas3Revisions()
//         {
//             var file = "package.json";
//             var hotSpotAnalysis = ActivityHotSpotAnalysis.Analyse(_blocks);
//             Assert.Equal(3, hotSpotAnalysis.HotSpots[file].NumberOfRevisions);
//         }
//     
//         [Fact]
//         public void ToCsvTest()
//         {
//             var hotSpotAnalysis = ActivityHotSpotAnalysis.Analyse(_blocks);
//             var csv = hotSpotAnalysis.ToCsv();
//             var expectedCsv = """ 
//                               entity,numberOfAuthors,numberOfRevisions
//                               package-lock.json,2,3
//                               package.json,2,3
//                               src/locales/en/translation.json,2,2
//                               src/components/activity-history/index.njk,1,1
//                               src/components/activity-history/tests/activity-history-controller.test.ts,1,1
//                               src/app.ts,1,1
//                               src/locales/cy/translation.json,1,1
//                               Dockerfile,1,1
//                               local.Dockerfile,1,1
//                               post-deploy-tests/Dockerfile,1,1
//
//                               """;
//             Assert.Equal(expectedCsv, csv);
//         }
    }

    public class ChangeHistogram
    {
        public static ChangeHistogram Analyse(List<CommitBlock> blocks)
        {
            throw new NotImplementedException();
        }

        public Dictionary<DateOnly, int> Months { get; }
    }
}