namespace Core.Components;

public class Motherboard : BaseComponent
{
    /// <summary>
    /// CPU Socket
    /// </summary>
    public string? Socket { get; set; }

    /// <summary>
    /// Form Factor
    /// </summary>
    public string? FormFactor { get; set; }

    /// <summary>
    /// Chipset
    /// </summary>
    public string? Chipset { get; set; }

    /// <summary>
    /// RAM Type 
    /// </summary>
    public string? MemoryType { get; set; }

    /// <summary>
    /// PCIE Version
    /// </summary>
    public double PCIEVersion { get; set; }

    /// <summary>
    /// Maximum Memory Capacity
    /// </summary>
    public double MaxMemoryCapacity { get; set; }
}
