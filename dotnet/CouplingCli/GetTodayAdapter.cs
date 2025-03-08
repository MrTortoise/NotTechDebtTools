using LogParser;

namespace CouplingCli;

public class GetTodayAdapter : IGetToday
{
    public DateTime Today => DateTime.Today;
}