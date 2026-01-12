namespace Core.Components;

using Core.Enums;

public class Storage : BaseComponent
{
    public bool? IsHDD { get; init; }
    public bool? IsSSD { get; init; }
    public bool? IsNVMe { get; init; }

    /// <summary>
    /// PcieType, if "Pcie5.0x4" =>> 5
    /// </summary>
    public PCIeVersion PcieType { get; init; }

    /// <summary>
    /// NVMe pcie lanes needed, if "Pcie5.0x4" >> 4
    /// </summary>
    public int? PcieLanes { get; init; }

    /// <summary>
    /// Read Speed in MB/s
    /// </summary>
    public double ReadSpeed { get; init; }

    /// <summary>
    /// Write speed in MB/s
    /// </summary>
    public double WriteSpeed { get; init; }
}
