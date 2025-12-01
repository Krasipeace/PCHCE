namespace Core.Components;

public class Motherboard : BaseComponent
{
    public string Socket { get; set; } = string.Empty;
    public string FormFactor { get; set; } = string.Empty;
    public string Chipset { get; set; } = string.Empty;
    public string MemoryType { get; set; } = string.Empty;
    public double PCIEVersion { get; set; }
    public double MaxMemoryCapacity { get; set; }
}
