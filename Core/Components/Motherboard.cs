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
    /// RAM Type - DDR3, DDR4, DDR5 etc.
    /// </summary>
    public string? MemoryType { get; set; }

    /// <summary>
    /// Maximum Memory Capacity
    /// </summary>
    public double MaxMemoryCapacity { get; set; }

    /// <summary>
    /// PCIE Version, if pcie5.0 >> 5.0
    /// </summary>
    public int PCIEVersion { get; set; } = 0;


    /// <summary>
    /// All Lanes available
    /// </summary>
    public int PcieLanes { get; set; } = 0;
}
