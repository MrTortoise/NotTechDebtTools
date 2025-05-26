namespace LogParser.Git;

/// <summary>
/// Represents a single file change with added/deleted lines and the file name.
/// This class is used within CommitBlock's Files list.
/// </summary>
public class File
{
    /// <summary>
    /// Gets the number of lines added in this file change.
    /// </summary>
    public int AddedLines { get; }

    /// <summary>
    /// Gets the number of lines deleted in this file change.
    /// </summary>
    public int DeletedLines { get; }

    /// <summary>
    /// Gets the name of the file that was changed.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="File"/> class.
    /// </summary>
    /// <param name="addedLines">The number of lines added.</param>
    /// <param name="deletedLines">The number of lines deleted.</param>
    /// <param name="fileName">The name of the file.</param>
    public File(int addedLines, int deletedLines, string fileName)
    {
        AddedLines = addedLines;
        DeletedLines = deletedLines;
        FileName = fileName;
    }
}