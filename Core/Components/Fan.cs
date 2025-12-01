namespace Core.Components;

public class Fan : BaseComponent
{
    /// <summary>
    /// Fanspeed in rounds per minute
    /// </summary>
    public double RPM { get; set; }

    /// <summary>
    /// Airflow in cubic feet per minute
    /// </summary>
    public double CFM { get; set; }
}
