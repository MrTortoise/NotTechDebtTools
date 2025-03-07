namespace LogParser;

public class File(int linesAdded, int linesRemoved, string fileName)
{
    public int LinesAdded { get; } = linesAdded;
    public int LinesRemoved { get; } = linesRemoved;
    public string FileName { get; } = fileName;
}