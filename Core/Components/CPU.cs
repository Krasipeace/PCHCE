namespace Core.Components;

public class CPU : BaseComponent
{
    public string Socket { get; set; } = string.Empty;
    public double Cores { get; set; }
    public double Threads { get; set; }
    public double BaseClock { get; set; }
    public double TurboClock { get; set; }
    public double CacheSize { get; set; }
}
