namespace Core.Components;

public class Case : BaseComponent
{
    /// <summary>
    /// Form factor of the case, presented as enum
    /// </summary>
    public CaseFormFactor CaseFormFactor { get; set; }

    /// <summary>
    /// Maximum height of the CPU cooler
    /// </summary>
    public double MaxCpuAirCoolerHeight { get; set; }

    /// <summary>
    /// Form factors of supported power supplies of the case
    /// </summary>
    public ICollection<PsuFormFactor>? SupportedPsuFormFactors { get; set; }

    /// <summary>
    /// Form factors of supported motherboards of the case
    /// </summary>
    public ICollection<MbFormFactor>? SupportedMbFormFactors { get; set; }

    /// <summary>
    /// Maximum length of the GPU (in mm)
    /// </summary>
    public double MaxGpuLength { get; set; }

    /// <summary>
    /// Max Radiator Size By Location: "Top", "Bottom", "Front"
    /// </summary>
    public Dictionary<string, List<string>> MaxRadiatorSizeByLocation { get; set; } = [];

}

/// <summary>
/// Mini Tower = 0, Cube Tower = 1, Mid Tower = 2, Full Tower = 3, Ultra Tower = 4
/// </summary>
public enum CaseFormFactor
{
    MiniTower = 0,
    CubeTower = 1,
    MidTower = 2,
    FullTower = 3,
    UltraTower = 4
}

/// <summary>
/// ATX = 0, SFX = 1, SFX-L = 2, TFX = 3, Flex-ATX = 4
/// </summary>
public enum PsuFormFactor
{
    ATX = 0,
    SFX = 1,
    SFX_L = 2,
    TFX = 3,
    FlexATX = 4
}

/// <summary>
/// mini-itx = 0, micro-atx = 1, atx = 2, eatx = 3, minidtx = 4, XL-ATX = 5, EEB = 6
/// </summary>
public enum MbFormFactor
{
    MiniITX = 0,
    MicroATX = 1,
    ATX = 2,
    eATX = 3,
    MiniDTX = 4,
    XLATX = 5,
    EEB = 6,
}