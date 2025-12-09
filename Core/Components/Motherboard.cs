namespace Core.Components;

public class Motherboard : BaseComponent
{
    /// <summary>
    /// CPU Socket
    /// </summary>
    public string? Socket { get; set; }

    /// <summary>
    /// Motherboard Form Factor, presented as enum
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
    /// PCIE Version, if pcie5.0 >> 5
    /// </summary>
    public int PCIEVersion { get; set; } = 0;


    /// <summary>
    /// All Lanes available, Default is 16.
    /// </summary>
    public int PcieLanes { get; set; } = 16;

    /// <summary>
    /// Default is 0.
    /// </summary>
    public int M2Slots { get; set; } = 0;
}
