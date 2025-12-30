namespace Core.Components;

using Core.Enums;

public class GPU : BaseComponent
{
    /// <summary>
    /// Only Nvidia GPUs have CUDA cores, so its detection between nvidia and other GPUs
    /// </summary>
    public bool? HasCuda { get; set; }

    /// <summary>
    /// Amount of Gpraphic Processing Units (Stream, CUDA etc.)
    /// </summary>
    public int Cores { get; init; }

    /// <summary>
    /// Amount of VRAM in GB
    /// </summary>
    public double Capacity { get; init; }

    /// <summary>
    /// Frequency of VRAM in MHz
    /// </summary>
    public double Frequency { get; init; }

    /// <summary>
    /// 128/256/384bit etc.
    /// </summary>
    public int Interface { get; init; }

    /// <summary>
    /// PCIe version of GPU
    /// </summary>
    public PCIeVersion PCIeVersion { get; init; } 

    /// <summary>
    /// PCIe Lanes of the GPU, if PCIe5.0 x8 >> 8
    /// </summary>
    public int LanesNeeded { get; init; } = 0;
}
