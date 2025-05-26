using LogParser.Git;

namespace LogParser;

public class GetTodayAdapter : IGetToday
{
    public DateTime Today => DateTime.Today;
}