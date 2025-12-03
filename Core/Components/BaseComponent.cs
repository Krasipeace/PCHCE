namespace Core.Components;

public abstract class BaseComponent
{
    /// <summary>
    /// Name of the component
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Power consumption in watts
    /// </summary>
    public double? PowerConsumption {  get; set; }

    /// <summary>
    /// Height dimension in mm
    /// </summary>
    public double? Height { get; set; }

    /// <summary>
    /// Length dimension in mm
    /// </summary>
    public double? Length { get; set; }

    /// <summary>
    /// Width dimension in mm
    /// </summary>
    public double? Width { get; set; } 
}