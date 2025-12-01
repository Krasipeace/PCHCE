namespace Core.Components;

public abstract class BaseComponent
{
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

    /// <summary>
    /// Used for known component that is better quality, but for some reason is ranked lower
    /// </summary>
    public int? UserRankIncrementor { get; set; }
}