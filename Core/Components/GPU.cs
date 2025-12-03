namespace Core.Components;

public class GPU : BaseComponent
{
    /// <summary>
    /// Only Nvidia GPUs have CUDA cores, so its detection between nvidia and other GPUs
    /// </summary>
    public bool? HasCuda { get; set; }

    /// <summary>
    /// Amount of Gpraphic Processing Units (Stream, CUDA etc.)
    /// </summary>
    public int Cores { get; set; }

    /// <summary>
    /// Amount of VRAM in GB
    /// </summary>
    public double Capacity { get; set; }

    /// <summary>
    /// Frequency of VRAM in MHz
    /// </summary>
    public double Frequency { get; set; }

    /// <summary>
    /// 128/256/384bit etc.
    /// </summary>
    public int Interface { get; set; }

    /// <summary>
    /// PCIe version of GPU, if Pcie5.0 x8 >> 5
    /// </summary>
    public int PCIeVersion { get; set; } = 0;

    /// <summary>
    /// PCIe Lanes of the GPU, if PCIe5.0 x8 >> 8
    /// </summary>
    public int LanesNeeded { get; set; } = 0;
}
