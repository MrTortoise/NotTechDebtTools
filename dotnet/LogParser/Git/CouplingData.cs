namespace LogParser.Git;

public class CouplingData(string source, string target, int frequency, int probability)
{
    public string Source { get; } = source;
    public string Target { get; } = target;
    public int Frequency { get; } = frequency;
    public float Probability { get; } = probability;
}