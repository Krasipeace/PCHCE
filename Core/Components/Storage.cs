namespace Core.Components;

public class Storage : BaseComponent
{
    public bool? IsHDD { get; set; }
    public bool? IsSSD { get; set; }
    public bool? IsNVMe { get; set; }

    /// <summary>
    /// PcieType, if "Pcie5.0x4" =>> 5.0
    /// </summary>
    public double? PcieType { get; set; }

    /// <summary>
    /// NVMe pcie lanes needed, if "Pcie5.0x4" >> 4
    /// </summary>
    public double? PcieLanes { get; set; }

    /// <summary>
    /// Read Speed in MB/s
    /// </summary>
    public double ReadSpeed { get; set; }

    /// <summary>
    /// Write speed in MB/s
    /// </summary>
    public double WriteSpeed { get; set; }
}
