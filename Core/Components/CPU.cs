namespace Core.Components;

public class CPU : BaseComponent
{
    /// <summary>
    /// Compatibility Evaluation Marker
    /// </summary>
    public string? Socket { get; init; }

    /// <summary>
    /// Amount of Cores(Physical)
    /// </summary>
    public int Cores { get; init; }

    /// <summary>
    /// Amount of Threads(Logical)
    /// </summary>
    public int Threads { get; init; }

    /// <summary>
    /// Base Clock in GHz
    /// </summary>
    public double BaseClockGhz { get; init; }

    /// <summary>
    /// Turbo/Boost Clock in GHz
    /// </summary>
    public double TurboClockGhz { get; init; }

    /// <summary>
    /// Cache Size in MB(Overall or L3)
    /// </summary>
    public int CacheSize { get; init; }
}
