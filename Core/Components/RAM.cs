namespace Core.Components;

public class RAM : BaseComponent
{
    /// <summary>
    /// Measured in MT/s
    /// </summary>
    public double Speed { get; set; }
    public double Voltage { get; set; }

    /// <summary>
    /// XMP / EXPO Profile
    /// </summary>
    public bool HasXMPorExpo { get; set; }

    /// <summary>
    /// Measured in ns, lower is better
    /// </summary>
    public double CASLatency { get; set; }

    /// <summary>
    /// Compatibility Evaluation Marker
    /// </summary>
    public string? Type { get; set; }
    /// <summary>
    /// Compatibility Evaluation Marker
    /// </summary>
    public bool? IsLowProfile { get; set; }
}
