namespace Core.Components;

using Core.Enums;

public class PowerSupply : BaseComponent
{
    /// <summary>
    /// The way PSU sizes are denoted, such as ATX
    /// </summary>
    public PsuFormFactor FormFactor { get; init; }

    /// <summary>
    /// Watts
    /// </summary> 
    public double Watts { get; init; }
    
    /// <summary>
    /// Efficiency in %
    /// </summary>
    public double Efficiency { get; init; }
}
