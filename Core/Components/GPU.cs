namespace Core.Components;

public class GPU : BaseComponent
{
    public string Type { get; set; } = string.Empty;
    public double Capacity { get; set; }
    public double Frequency { get; set; }
    public string Interface { get; set; } = string.Empty;
}
