namespace Core;

using Core.Components;

public class CompatibilityEvaluator
{
    /// <summary>
    /// PCIe Version enumarable for bottleneck compares
    /// </summary>
    private enum PCIeVersion
    {
        PCIe_3_0 = 3,
        PCIe_4_0 = 4,
        PCIe_5_0 = 5,
        PCIe_6_0 = 6
    }

    /// <summary>
    /// Compare sockets of cpu and motherboard
    /// </summary>
    /// <param name="cpu"></param>
    /// <param name="motherboard"></param>
    /// <returns></returns>
    public bool CompareCpuMotherboardSockets(CPU cpu, Motherboard motherboard) => cpu.Socket == motherboard.Socket;

    /// <summary>
    /// Compare GPU power consumption to PSU
    /// </summary>
    /// <param name="gpu">gpu power consumption</param>
    /// <param name="psu">power supply wattage</param>
    /// <returns></returns>
    public bool CompareGpuPsuWatts(GPU gpu, PowerSupply psu) => gpu.PowerConsumption <= psu.Watts;

    /// <summary>
    /// Compare Case dimensions(form factor) and motherboard dimensions(form factor)
    /// </summary>
    /// <param name="case">case form factor, from enum CaseFormFactor</param>
    /// <param name="motherboard">mb form factor, from enum MbFormFactor</param>
    /// <returns>Returns True, if compatible. False, if null or incompatible</returns>
    public bool CompareCaseMotherBoardFormFactor(Case @case, Motherboard motherboard) => (int)@case.CaseFormFactor >= (int)motherboard.MbFormFactor;

    /// <summary>
    /// Compare RAM and motherboard RAM types, it ignores upper/down cases, dashes, spaces
    /// </summary>
    /// <param name="ram">Type is string</param>
    /// <param name="motherboard">MemoryType is string</param>
    /// <returns></returns>
    public bool CompareRamMotherboardMemoryType(RAM ram, Motherboard motherboard)
    {
        if (ram == null || motherboard == null || ram.Type == null || motherboard.MemoryType == null) return false;

        return Normalize(ram.Type) == Normalize(motherboard.MemoryType);
    }

    /// <summary>
    /// Compares PCIe versions of GPU and motherboard and returns a compatibility score (0-100).
    /// </summary>
    /// <param name="gpu"></param>
    /// <param name="motherboard"></param>
    /// <returns>Returns score from 0(worst) to 100(best). Also returns 0 if any of the input objects are null.</returns>
    public double CompareGpuMotherboardInterfacesScore(GPU gpu, Motherboard motherboard)
    {
        if (gpu == null ||  motherboard == null || gpu?.LanesNeeded == null || motherboard?.PcieLanes == null) return 0;

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
    /// Get relative bandwidth power (e.g., 3.0=1, 4.0=2, 5.0=4)
    /// </summary>
    private double GetRelativeBandwidthMultiplier(PCIeVersion version) => Math.Pow(2, (int)version - 3);

    /// <summary>
    /// Check string for upper/down/spaces/dashes and ignores them
    /// </summary>
    /// <param name="s">the string to check</param>
    /// <returns>Returns new string to uppercase without spaces/dashes</returns>
    private static string Normalize(string s) => new string(s?.Where(c => !char.IsWhiteSpace(c) && c != '-').ToArray()).ToUpperInvariant();
}
