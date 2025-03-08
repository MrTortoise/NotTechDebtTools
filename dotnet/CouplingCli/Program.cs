using System.Diagnostics;
using CouplingCli;
using LogParser;
using File = LogParser.File;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Extracting logfiles and running analysis");
if (args.Length != 3)
    throw new ArgumentException(
        "need to pass a path to a git repo as first argument. Second is YYYY-MM-DD format, third is output folder");

var path = ExpandPath(args[0]);
var date = args[1];
var output = ExpandPath(args[2]);

var script = $"run_maat.sh {path} {date} {output}";
RunBashScript(script);
var gitLog = System.IO.File.ReadAllText($"{output}/logfile.log");

var blocks = BlockParser.GetBlocks(gitLog);

var ages = ActiveFileIdentificationAnalysis.Analyse(blocks, new GetTodayAdapter());
System.IO.File.WriteAllText($"{output}/age.csv", ages.ToCsv());

var authorChurn = AuthorChurn.Analyse(blocks);
System.IO.File.WriteAllText($"{output}/author-churn.csv", authorChurn.ToCsv());

var entityChurn = EntityChurn.Analyse(blocks);
System.IO.File.WriteAllText($"{output}/entity-churn.csv", entityChurn.ToCsv());

return 0;

static string ExpandPath(string path)
{
    if (string.IsNullOrWhiteSpace(path))
    {
        throw new ArgumentException("Path cannot be null or empty.", nameof(path));
    }

    if (path.StartsWith("~"))
    {
        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(homePath, path.TrimStart('~', Path.DirectorySeparatorChar));
    }

    return Path.GetFullPath(path);
} 

static void RunBashScript(string scriptPath)
{
    try
    {
        Process process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = scriptPath;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
            
        process.WaitForExit();

        Console.WriteLine("Output:\n" + output);
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine("Error:\n" + error);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception: " + ex.Message);
    }
}