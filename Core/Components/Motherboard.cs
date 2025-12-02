namespace Core.Components;

public class Motherboard : BaseComponent
{
    /// <summary>
    /// CPU Socket
    /// </summary>
    public string Socket { get; set; } = string.Empty;

    /// <summary>
    /// Form Factor
    /// </summary>
    public string FormFactor { get; set; } = string.Empty;

    /// <summary>
    /// Chipset
    /// </summary>
    public string Chipset { get; set; } = string.Empty;

    /// <summary>
    /// RAM Type 
    /// </summary>
    public string MemoryType { get; set; } = string.Empty;

    /// <summary>
    /// PCIE Version
    /// </summary>
    public double PCIEVersion { get; set; }

    /// <summary>
    /// Maximum Memory Capacity
    /// </summary>
    public double MaxMemoryCapacity { get; set; }
}
