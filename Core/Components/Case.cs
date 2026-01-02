namespace Core.Components;

using Core.Enums;

public class Case : BaseComponent
{
    /// <summary>
    /// Form factor of the case, presented as enum
    /// </summary>
    public CaseFormFactor CaseFormFactor { get; init; }

    /// <summary>
    /// Maximum height of the CPU cooler
    /// </summary>
    public double MaxCpuAirCoolerHeight { get; init; }

    /// <summary>
    /// Maximum length of the GPU (in mm)
    /// </summary>
    public double MaxGpuLength { get; init; }

    /// <summary>
    /// Form factors of supported power supplies of the case
    /// </summary>
    public IReadOnlyCollection<PsuFormFactor> SupportedPsuFormFactors { get; init; } = [];

    /// <summary>
    /// Form factors of supported motherboards of the case
    /// </summary>
    public IReadOnlyCollection<MbFormFactor> SupportedMbFormFactors { get; init; } = [];

    /// <summary>
    /// Max Radiator Size By Location: Front, Top, Bottom, Rear
    /// </summary>
    public IReadOnlyDictionary<RadiatorLocation, IReadOnlyCollection<RadiatorSize>> MaxRadiatorSizeByLocation { get; init; } = new Dictionary<RadiatorLocation, IReadOnlyCollection<RadiatorSize>>();
}
