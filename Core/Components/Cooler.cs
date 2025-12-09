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
    /// Converts Component Length into string. Used for Liquid Cooler Comparison.
    /// </summary>
    public string? LiquidCoolerLengthMM
    {
        get => Length?.ToString();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Length = null;
                return;
            }

            if (double.TryParse(value, out double result)) Length = result;
            else Length = null;
        }
    }
}
