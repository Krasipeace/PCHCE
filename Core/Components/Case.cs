namespace Core.Components;

public class Case : BaseComponent
{
    public string CaseFormFactor { get; set; } = string.Empty;
    public double MaxCpuCoolerHeight { get; set; }
    public string PsuFormFactor { get; set; } = string.Empty;
    public string MotherboardFormFactor { get; set; } = string.Empty;
    public double MaxGpuLength { get; set; }
    public bool? CanFit420mmRadioatorOnTopPanel { get; set; }
    public bool? CanFit420mmRadioatorOnFrontPanel { get; set; }
    public bool? CanFit420mmRadioatorOnBottomPanel { get; set; }
    public bool? CanFit360mmRadioatorOnTopPanel { get; set; }
    public bool? CanFit360mmRadioatorOnFrontPanel { get; set; }
    public bool? CanFit360mmRadioatorOnBottomPanel { get; set; }
    public bool? CanFit280mmRadioatorOnTopPanel { get; set; }
    public bool? CanFit280mmRadioatorOnFrontPanel { get; set; }
    public bool? CanFit280mmRadioatorOnBottomPanel { get; set; }
    public bool? CanFit120mmRadioatorOnTopPanel { get; set; }
    public bool? CanFit120mmRadioatorOnFrontPanel { get; set; }
    public bool? CanFit120mmRadioatorOnBottomPanel { get; set; }
    public bool? CanFit240mmRadioatorOnTopPanel { get; set; }
    public bool? CanFit240mmRadioatorOnFrontPanel { get; set; }
    public bool? CanFit240mmRadioatorOnBottomPanel { get; set; }
}