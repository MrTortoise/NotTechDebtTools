namespace LogParser.Test.File;

public class LinesOfCodeAnalysisTest
{
    [Fact]
    public void TestFileShouldReturn70Lines()
    {
        var fileName = $"File/TestFile.txt";
        var file = System.IO.File.ReadAllText(fileName);
        var results = CodeFileStats.Analyse(fileName,file);
        Assert.Equal(70, results.LinesOfCode);
    }
    
}

public class CodeFileStats(string fileName, int linesOfCode)
{
    public string FileName { get; } = fileName;
    public int LinesOfCode { get; } = linesOfCode;

    public static CodeFileStats Analyse(string fileName, string text)
    {
        var linesOfCode = text.Split('\n').Length;
        
        return new CodeFileStats(fileName, linesOfCode);
    }
}