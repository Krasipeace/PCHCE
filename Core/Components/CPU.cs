namespace Core.Components;

public class CPU : BaseComponent
{
    /// <summary>
    /// Compatibility Evaluation Marker
    /// </summary>
    public string? Socket { get; set; }

    /// <summary>
    /// Amount of Cores(Physical)
    /// </summary>
    public int Cores { get; set; }

    /// <summary>
    /// Amount of Threads(Logical)
    /// </summary>
    public int Threads { get; set; }

    /// <summary>
    /// Base Clock in GHz
    /// </summary>
    public double BaseClockGhz { get; set; }

    /// <summary>
    /// Turbo/Boost Clock in GHz
    /// </summary>
    public double TurboClockGhz { get; set; }

    /// <summary>
    /// Cache Size in MB(Overall or L3)
    /// </summary>
    public int CacheSize { get; set; }
}
