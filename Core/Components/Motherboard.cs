namespace Core.Components;

using Core.Enums;

public class Motherboard : BaseComponent
{
    /// <summary>
    /// CPU Socket
    /// </summary>
    public string? Socket { get; init; }

    /// <summary>
    /// Motherboard Form Factor, presented as enum
    /// </summary>
    public MbFormFactor FormFactor { get; init; }

    /// <summary>
    /// Chipset
    /// </summary>
    public string? Chipset { get; init; }

    /// <summary>
    /// RAM Type - DDR3, DDR4, DDR5 etc.
    /// </summary>
    public MemoryType MemoryType { get; init; }

    /// <summary>
    /// Maximum Memory Capacity
    /// </summary>
    public double MaxMemoryCapacity { get; init; }

    /// <summary>
    /// PCIE Version
    /// </summary>
    public PCIeVersion PCIEVersion { get; init; } 


    /// <summary>
    /// All Lanes available, Default is 16.
    /// </summary>
    public int PcieLanes { get; init; } = 16;

    /// <summary>
    /// Default is 0.
    /// </summary>
    public int M2Slots { get; init; } = 0;
}
