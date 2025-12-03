namespace Core.Components;

public class PowerSupply : BaseComponent
{
    /// <summary>
    /// The way PSU sizes are denoted, such as ATX
    /// </summary>
    public string? FormFactor { get; set; }

    /// <summary>
    /// Watts
    /// </summary> 
    public double Watts { get; set; }
    
    /// <summary>
    /// Efficiency in %
    /// </summary>
    public double Efficiency { get; set; }
}
