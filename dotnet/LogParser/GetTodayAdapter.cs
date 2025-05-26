using LogParser;
using LogParser.Git;

namespace CouplingCli;

public class GetTodayAdapter : IGetToday
{
    public DateTime Today => DateTime.Today;
}