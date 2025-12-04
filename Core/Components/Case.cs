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
    public double MaxCpuCoolerHeight { get; set; }

    /// <summary>
    /// Form factor of the power supply
    /// </summary>
    public string PsuFormFactor { get; set; } = string.Empty;

    /// <summary>
    /// Maximum length of the GPU (in mm)
    /// </summary>
    public double MaxGpuLength { get; set; }

    /// <summary>
    /// Can the case fit a 420mm radiator on the top panel?
    /// </summary>
    public bool? CanFit420mmRadioatorOnTopPanel { get; set; }

    /// <summary>
    /// Can the case fit a 420mm radiator on the front panel?
    /// </summary>
    public bool? CanFit420mmRadioatorOnFrontPanel { get; set; }

    /// <summary>
    /// Can the case fit a 420mm radiator on the bottom panel?
    /// </summary>
    public bool? CanFit420mmRadioatorOnBottomPanel { get; set; }

    /// <summary>
    /// Can the case fit a 360mm radiator on the top panel?
    /// </summary>
    public bool? CanFit360mmRadioatorOnTopPanel { get; set; }

    /// <summary>
    /// Can the case fit a 360mm radiator on the front panel?
    /// </summary>
    public bool? CanFit360mmRadioatorOnFrontPanel { get; set; }

    /// <summary>
    /// Can the case fit a 360mm radiator on the bottom panel?
    /// </summary>
    public bool? CanFit360mmRadioatorOnBottomPanel { get; set; }

    /// <summary>
    /// Can the case fit a 280mm radiator on the top panel?
    /// </summary>
    public bool? CanFit280mmRadioatorOnTopPanel { get; set; }

    /// <summary>
    /// Can the case fit a 280mm radiator on the front panel?
    /// </summary>
    public bool? CanFit280mmRadioatorOnFrontPanel { get; set; }

    /// <summary>
    /// Can the case fit a 280mm radiator on the bottom panel?
    /// </summary>
    public bool? CanFit280mmRadioatorOnBottomPanel { get; set; }

    /// <summary>
    /// Can the case fit a 120mm radiator on the top panel?
    /// </summary>
    public bool? CanFit120mmRadioatorOnTopPanel { get; set; }

    /// <summary>
    /// Can the case fit a 120mm radiator on the front panel?
    /// </summary>
    public bool? CanFit120mmRadioatorOnFrontPanel { get; set; }

    /// <summary>
    /// Can the case fit a 120mm radiator on the bottom panel?
    /// </summary>
    public bool? CanFit120mmRadioatorOnBottomPanel { get; set; }

    /// <summary>
    /// Can the case fit a 240mm radiator on the top panel?
    /// </summary>
    public bool? CanFit240mmRadioatorOnTopPanel { get; set; }

    /// <summary>
    /// Can the case fit a 240mm radiator on the front panel?
    /// </summary>
    public bool? CanFit240mmRadioatorOnFrontPanel { get; set; }

    /// <summary>
    /// Can the case fit a 240mm radiator on the bottom panel?
    /// </summary>
    public bool? CanFit240mmRadioatorOnBottomPanel { get; set; }

}

/// <summary>
/// Mini-itx = 0, microatx = 1, midtower = 2, fulltower = 3
/// </summary>
public enum CaseFormFactor
{
    MiniITX = 0,
    MicroATX = 1,
    MidTower = 2,
    FullTower = 3
}
