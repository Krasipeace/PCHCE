namespace Core;

using Core.Components;

public class CompatibilityEvaluator
{


    /// <summary>
    /// Compare sockets of cpu and motherboard
    /// </summary>
    /// <param name="cpu"></param>
    /// <param name="motherboard"></param>
    /// <returns></returns>
    public bool CompareCpuMotherboardSockets(CPU cpu, Motherboard motherboard) => cpu.Socket == motherboard.Socket;

    /// <summary>
    /// Compare PCIe versions of GPU and motherboard
    /// </summary>
    /// <param name="gpu"></param>
    /// <param name="motherboard"></param>
    /// <returns></returns>
    public bool CompareGpuMotherboardInterfaces(GPU gpu, Motherboard motherboard)
    {
        bool isPcieCompatible = gpu.PCIeVersion <= motherboard.PCIEVersion;
        bool isPcieLanesEnough = gpu.LanesNeeded <= motherboard.PcieLanes;

        return isPcieCompatible && isPcieLanesEnough;
    }

    /// <summary>
    /// Compare GPU power consumption to PSU
    /// </summary>
    /// <param name="gpu">gpu power consumption</param>
    /// <param name="psu">power supply wattage</param>
    /// <returns></returns>
    public bool CompareGpuPsuWatts(GPU gpu, PowerSupply psu) => gpu.PowerConsumption <= psu.Watts;

    /// <summary>
    /// Compare RAM and motherboard DDRAM
    /// </summary>
    /// <param name="ram"></param>
    /// <param name="motherboard"></param>
    /// <returns></returns>
    public bool CompareRamMotherboardDDRAM(RAM ram, Motherboard motherboard) => ram.Type == motherboard.MemoryType;

    /// <summary>
    /// Compares PCIe versions of GPU and motherboard and returns a compatibility score (0-100).
    /// </summary>
    /// <param name="gpu"></param>
    /// <param name="motherboard"></param>
    /// <returns></returns>
    public double CompareGpuMotherboardInterfacesScore(GPU gpu, Motherboard motherboard)
    {
        double actualLanesUsed = Math.Min(gpu.LanesNeeded, motherboard.PcieLanes);

        if (gpu.LanesNeeded == 0) return 0;

        double laneScore = (actualLanesUsed / gpu.LanesNeeded) * 100;
        PCIeVersion bottleneckVersion = (PCIeVersion)Math.Min(gpu.PCIeVersion, motherboard.PCIEVersion);
        double bottleneckMultiplier = GetRelativeBandwidthMultiplier(bottleneckVersion);
        double gpuRequiredMultiplier = GetRelativeBandwidthMultiplier((PCIeVersion)gpu.PCIeVersion);
        double versionScore = (bottleneckMultiplier / gpuRequiredMultiplier) * 100;
        double finalScore = Math.Min(laneScore, versionScore);

        if (motherboard.PcieLanes < 4) return 0;

        return finalScore;
    }

    /// <summary>
    /// PCIe Version enumarable for bottleneck comparator
    /// </summary>
    private enum PCIeVersion
    {
        PCIe_3_0 = 3,
        PCIe_4_0 = 4,
        PCIe_5_0 = 5,
        PCIe_6_0 = 6
    }

    /// <summary>
    /// Helper function to get relative bandwidth power (e.g., 3.0=1, 4.0=2, 5.0=4)
    /// </summary>
    private double GetRelativeBandwidthMultiplier(PCIeVersion version) => Math.Pow(2, (int)version - 3);
}
