namespace LogParser;

public class File(int linesAdded, int linesDeleted, string fileName)
{
    public int LinesAdded { get; } = linesAdded;
    public int LinesDeleted { get; } = linesDeleted;
    public string FileName { get; } = fileName;
}