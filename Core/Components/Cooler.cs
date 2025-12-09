namespace Core.Components;

public class Cooler : BaseComponent
{
    /// <summary>
    /// True - air cooled, false - liquid cooled
    /// </summary>
    public bool IsAir { get; set; }

    /// <summary>
    /// Fanspeed in rounds per minute
    /// </summary>
    public double RPM { get; set; }

    /// <summary>
    /// Airflow in cubic feet per minute
    /// </summary>
    public double CFM { get; set; }

    /// <summary>
    /// Radiator with 1 fan of 120mm
    /// </summary>
    public bool RadiatorIs120 { get; set; } = false;

    /// <summary>
    /// Radiator with 1 fan of 140mm
    /// </summary>
    public bool RadiatorIs140 { get; set; } = false;

    /// <summary>
    /// Radiator with 2 fans of 120mm
    /// </summary>
    public bool RadiatorIs240 { get; set; } = false;

    /// <summary>
    /// Radiator with 2 fans of 140mm each
    /// </summary>
    public bool RadiatorIs280 { get; set; } = false;

    /// <summary>
    /// Radiator of 3 fans of 120mm each
    /// </summary>
    public bool RadiatorIs360 { get; set; } = false;

    /// <summary>
    /// Radiator of 3 fanse 140mm each
    /// </summary>
    public bool RadiatorIs420 { get; set; } = false;
}
