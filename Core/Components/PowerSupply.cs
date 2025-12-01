namespace Core.Components;

public class PowerSupply : BaseComponent
{
    /// <summary>
    /// Compatibility Evaluation Marker
    /// </summary>
    public string? FormFactor { get; set; } 
    public double Watts { get; set; }
    public double Efficiency { get; set; }
}
