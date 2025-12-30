namespace Core.Components;

using Core.Enums;

public class RAM : BaseComponent
{
    /// <summary>
    /// Measured in MT/s
    /// </summary>
    public double Speed { get; init; }

    /// <summary>
    /// Measured in Volts
    /// </summary>
    public double Voltage { get; init; }

    /// <summary>
    /// XMP / EXPO Profile
    /// </summary>
    public bool HasXMPorExpo { get; init; }

    /// <summary>
    /// Measured in ns, lower is better
    /// </summary>
    public double CASLatency { get; init; }

    /// <summary>
    /// RAM type - DDR3, DDR4, DDR5, etc.
    /// </summary>
    public MemoryType Type { get; set; }

    /// <summary>
    /// True, if RAM is Low Profile(compatible with all Air Coolers)
    /// </summary>
    public bool? IsLowProfile { get; set; }
}
