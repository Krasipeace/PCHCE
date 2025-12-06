namespace Core;

using Core.Components;

public class CompatibilityEvaluator
{
    /// <summary>
    /// PCIe Version enumarable
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
    public bool CompareCpuMotherboardSockets(CPU cpu, Motherboard motherboard)
    {
        if (cpu == null || cpu.Socket == null || 
            motherboard == null || motherboard.Socket == null) 
            return false;

        return Normalize(cpu.Socket) == Normalize(motherboard.Socket);
    }

    /// <summary>
    /// Compare GPU power consumption to pc system PSU
    /// </summary>
    /// <param name="gpu">gpu power consumption</param>
    /// <param name="psu">power supply wattage</param>
    /// <returns></returns>
    public bool CompareGpuPsuWatts(GPU gpu, PowerSupply psu)
    {
        if (gpu == null || gpu.PowerConsumption == null || 
            psu == null || psu?.Watts == null) 
            return false;

        return gpu.PowerConsumption <= psu.Watts;
    }

    /// <summary>
    /// Compares the total estimated power consumption of the core PC components (CPU, GPU, Motherboard, and a fixed margin for peripherals) against the maximum wattage of the provided Power Supply Unit (PSU).
    /// </summary>
    /// <param name="cpu">The CPU component.</param>
    /// <param name="gpu">The GPU component.</param>
    /// <param name="motherboard">The Motherboard component.</param>
    /// <param name="psu">The Power Supply Unit component.</param>
    /// <returns>True, if PSU watts will be enough to power up the system.</returns>
    public bool CompareAllToPsuWatts(CPU cpu, GPU gpu, Motherboard motherboard, PowerSupply psu)
    {
        if (cpu == null || cpu.PowerConsumption == null || 
            gpu == null || gpu.PowerConsumption == null || 
            motherboard == null || motherboard.PowerConsumption == null ||
            psu == null || psu?.Watts == null) 
            return false;

        double? allComponentsWatts = cpu.PowerConsumption + gpu.PowerConsumption + motherboard.PowerConsumption + 100.0;

        return allComponentsWatts <= psu.Watts;
    }

    /// <summary>
    /// Compares the total estimated power consumption of the core PC components (CPU, GPU, Motherboard, and a fixed margin for peripherals) against the maximum wattage of the provided Power Supply Unit (PSU).
    /// </summary>
    /// <param name="cpu">The CPU component.</param>
    /// <param name="gpu">The GPU component.</param>
    /// <param name="motherboard">The Motherboard component.</param>
    /// <param name="psu">The Power Supply Unit component.</param>
    /// <param name="customAdditionalWatts">Add by user watts, used mainly for enthusiast level of pc systems measurement.</param>
    /// <returns>True, if PSU watts will be enough to power up the system.</returns>
    public bool CompareAllToPsuWatts(CPU cpu, GPU gpu, Motherboard motherboard, PowerSupply psu, double? customAdditionalWatts)
    {
        if (cpu == null || cpu.PowerConsumption == null ||
            gpu == null || gpu.PowerConsumption == null ||
            motherboard == null || motherboard.PowerConsumption == null ||
            psu == null || psu?.Watts == null || customAdditionalWatts == null)
            return false;

        double? allComponentsWatts = cpu.PowerConsumption + gpu.PowerConsumption + motherboard.PowerConsumption + customAdditionalWatts;

        return allComponentsWatts <= psu.PowerConsumption;
    }

    /// <summary>
    /// Compare Case dimensions(form factor) and motherboard dimensions(form factor)
    /// </summary>
    /// <param name="case">case form factor, from enum CaseFormFactor</param>
    /// <param name="motherboard">mb form factor, from enum MbFormFactor</param>
    /// <returns>Returns True, if compatible. False, if null or incompatible</returns>
    public bool CompareCaseMotherBoardFormFactor(Case @case, Motherboard motherboard)
    {
        if (@case == null || @case?.CaseFormFactor == null || 
            motherboard == null || motherboard?.MbFormFactor == null) 
            return false;

        return (int)@case.CaseFormFactor >= (int)motherboard.MbFormFactor;
    }

    /// <summary>
    /// Compare RAM and motherboard RAM types, it ignores upper/down cases, dashes, spaces
    /// </summary>
    /// <returns>True if compatible, false - not</returns>
    public bool CompareRamMotherboardMemoryType(RAM ram, Motherboard motherboard)
    {
        if (ram == null || motherboard == null || 
            ram.Type == null || motherboard.MemoryType == null) 
            return false;

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
        if (gpu == null ||  motherboard == null || 
            gpu?.LanesNeeded == null || motherboard?.PcieLanes == null) 
            return 0;

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
    /// Get relative bandwidth power
    /// </summary>
    /// <param name="version">the selected version as enumable</param>
    /// <returns>Math value of pcie version</returns>
    private double GetRelativeBandwidthMultiplier(PCIeVersion version) => Math.Pow(2, (int)version - 3);

    /// <summary>
    /// Check string for upper/down/spaces/dashes and ignores them
    /// </summary>
    /// <param name="s">the string to check</param>
    /// <returns>Returns new string to uppercase without spaces/dashes</returns>
    private string Normalize(string s) => new string(s?.Where(c => !char.IsWhiteSpace(c) && c != '-').ToArray()).ToUpperInvariant();
}
