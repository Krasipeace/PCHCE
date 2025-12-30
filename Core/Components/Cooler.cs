namespace Core.Components;

using Core.Enums;

public class Cooler : BaseComponent
{
    /// <summary>
    /// True - air cooled, false - liquid cooled
    /// </summary>
    public bool IsAir { get; set; }

    /// <summary>
    /// Fanspeed in rounds per minute
    /// </summary>
    public double RPM { get; init; }

    /// <summary>
    /// Airflow in cubic feet per minute
    /// </summary>
    public double CFM { get; init; }

    /// <summary>
    /// Size of Liquid Cooler Radiator in mm
    /// </summary>
    public RadiatorSize? RadiatorSize { get; init; }
}
