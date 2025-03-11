using System.Collections;

namespace LogParser;

public class CouplingSource(string fileName) : IEnumerable<CouplingData>
{
    private string FileName { get; } = fileName;
    private int _numberOfCouplings = 0;
    private Dictionary<string, int> CoupledTo { get; set; } = [];

    public void AddCouplingTo(IEnumerable<string> fileNames)
    {
        _numberOfCouplings += 1;
        foreach (var name in fileNames)
        {
            if (!CoupledTo.TryAdd(name, 1))
            {
                CoupledTo[name]++;
            }
        }
    }

    public CouplingData this[string fileName]
    {
        get
        {
            var item = CoupledTo[fileName];
            return new CouplingData(FileName, fileName, item, Convert.ToInt32((float)item * 100 / _numberOfCouplings));
        }
    }

    public IEnumerator<CouplingData> GetEnumerator()
    {
        foreach (var item in CoupledTo)
        {
            yield return new CouplingData(FileName, item.Key, item.Value,
                Convert.ToInt32((float)item.Value * 100 / _numberOfCouplings));
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}