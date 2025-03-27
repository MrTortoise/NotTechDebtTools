using LogParser.Git;

namespace LogParser.Test.Git
{
    public class MergeAnalysisTest
    {
        private const string ExampleData = """
                                           --86c33bef--2022-10-31--Alex Wilson--Merge pull request #659 from alphagov/bau-remove-old-logging-endpoint
                                           --7107ae2b--2022-10-31--Alex Wilson--BAU: Stop writing to old logging endpoint
                                           0	1	ci/terraform/build.tfvars
                                           0	1	ci/terraform/integration.tfvars
                                           0	1	ci/terraform/production.tfvars
                                           0	1	ci/terraform/sandpit.tfvars

                                           --4d17e8b6--2022-10-28--huw--Merge pull request #653 from alphagov/bau-take-over-app
                                           --bf231e7f--2022-10-28--DTaylorGDS--Merge pull request #656 from alphagov/AUT-824/fix-international-phone-number-validation-rule
                                           --0600eb64--2022-10-28--Dominic Taylor--AUT-824: Fix international phone number validation rule
                                           12	4	src/utils/phone-number.ts
                                           8	0	test/unit/utils/phone-number.test.ts

                                           --8d674a17--2022-10-28--huw--Merge pull request #652 from alphagov/adrs
                                           --047996af--2022-10-27--Huw Diprose--BAU: Record decision to take over app
                                           61	0	docs/adr/0002-move-to-secure-pipelines.md

                                           --a35a9ef0--2022-10-27--Huw Diprose--GUA-477: Start recording ADRs locally
                                           7	0	docs/adr/0001-record-adrs.md

                                           --1c1c559f--2022-10-26--DTaylorGDS--Merge pull request #650 from alphagov/revert-641-DCP-88-WelshInProd
                                           --dc805525--2022-10-26--DTaylorGDS--Revert "DCP-88: Enable Welsh language in production"
                                           1	3	ci/terraform/production.tfvars

                                           --b509a04a--2022-10-26--huw--Merge pull request #649 from alphagov/turnOnWelsh
                                           --f2bd80cd--2022-10-26--Huw Diprose--Set Welsh as Default on in the management app
                                           1	1	ci/tasks/deploy-account-management.yml

                                           --cb3fe75a--2022-10-26--huw--Merge pull request #648 from alphagov/modify-codeowners
                                           --68a03337--2022-10-26--huw--Merge pull request #641 from alphagov/DCP-88-WelshInProd
                                           --56c96f4a--2022-10-26--Huw Diprose--BAU: Add alpha and bravo teams to codeowners
                                           1	1	CODEOWNERS

                                           --d80ab58e--2022-10-25--domiliauskas--ATB-63 WIP NLB, ACL Association, REST APIGW
                                           212	85	deploy/template.yaml

                                           """;
    
        private readonly List<CommitBlock> _blocks = BlockParser.GetBlocks(ExampleData);

        [Fact]
        public void WhoIsMergingHas3People()
        {
            var results = MergeAnalysis.Analyse(blocks: _blocks);
            Assert.Equal(3, results.MergeAnalysisEntries.Count);
        }
    
        [Fact]
        public void HuwHas5Merges()
        {
            var results = MergeAnalysis.Analyse(blocks: _blocks);
            Assert.Equal(5, results.MergeAnalysisEntries["huw"].NumberOfMerges);
        }
    }

    public class MergeAnalysis(Dictionary<string, MergeAnalysisEntry> mergeAnalysisEntries)
    {
        public Dictionary<string, MergeAnalysisEntry> MergeAnalysisEntries { get; } = mergeAnalysisEntries;

        public static MergeAnalysis Analyse(List<CommitBlock> blocks)
        {
            var mergers = new Dictionary<string, MergeAnalysisEntry>();
            foreach (var block in blocks)
            {
                foreach (var merger in block.Mergers)
                {
                    if (!mergers.ContainsKey(merger))
                    {
                        mergers[merger] = new MergeAnalysisEntry(merger);
                    }
                    else
                    {
                        var entry = mergers[merger];
                        mergers[merger] = new MergeAnalysisEntry(merger, entry.NumberOfMerges+1);
                    }
                }
            }

            return new MergeAnalysis(mergers);
        }
    }

    public class MergeAnalysisEntry(string merger, int numberOfMerges = 1)
    {
        public string Merger { get; } = merger;
        public int NumberOfMerges { get; } = numberOfMerges;
    }
}