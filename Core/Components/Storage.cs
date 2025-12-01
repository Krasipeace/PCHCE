namespace Core.Components;

public class Storage : BaseComponent
{
    public bool? IsHDD { get; set; }
    public bool? IsSSD { get; set; }
    public bool? IsNVMe { get; set; }

    /// <summary>
    /// Compatibility Evaluation Marker
    /// </summary>
    public string? PcieType { get; set; }

    /// <summary>
    /// Read Speed in MB/s
    /// </summary>
    public double ReadSpeed { get; set; }

    /// <summary>
    /// Write speed in MB/s
    /// </summary>
    public double WriteSpeed { get; set; }
}
