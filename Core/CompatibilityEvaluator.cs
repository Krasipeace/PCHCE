namespace Core;

using Core.Components;

using System.Linq;

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

    #region Compare component characteristics
    /// <summary>
    /// Compare Case form factor and motherboard form factor
    /// </summary>
    /// <param name="case">case form factor</param>
    /// <param name="motherboard">mb form factor</param>
    /// <returns>Returns True, if motherboard form factor is in case's supported motherboards list</returns>
    public bool CompareCaseMotherBoardFormFactor(Case @case, Motherboard motherboard)
    {
        if (@case == null || @case?.SupportedMbFormFactors == null ||
            motherboard == null || motherboard?.FormFactor == null)
            return false;

        Normalize(motherboard.FormFactor);

        if (!Enum.TryParse<MbFormFactor>(motherboard.FormFactor, true, out var mbFormFactor)) return false;

        return @case.SupportedMbFormFactors.Contains(mbFormFactor);
    }

    /// <summary>
    /// Compare PC case power supply form factors to power supply form factor.
    /// </summary>
    /// <param name="case">the pc case input.</param>
    /// <param name="psu">the power supply unit input.</param>
    /// <returns></returns>
    public bool CompareCasePsuFormFactor(Case @case, PowerSupply psu)
    {
        if (@case?.SupportedPsuFormFactors == null || psu?.FormFactor == null) return false;

        Normalize(psu.FormFactor);

        if (!Enum.TryParse<PsuFormFactor>(psu.FormFactor, true, out var psuFactor)) return false;

        return @case.SupportedPsuFormFactors.Contains(psuFactor);
    }

    /// <summary>
    /// Check Max PC Case Gpu Length to actual GPU Length
    /// </summary>
    /// <param name="case">the pc case input</param>
    /// <param name="gpu">the gpu input</param>
    /// <returns>Returns True, if GPU can fit the case, False - otherwise</returns>
    public bool CompareCaseGpuLength(Case @case, GPU gpu)
    {
        if (@case?.MaxGpuLength == null || gpu.Length == null) return false;

        return @case.MaxGpuLength >= gpu.Length;
    }

    /// <summary>
    /// Compare Air Cooler Height to Case Air Cooler compatibility.
    /// Compare Liquid Cooler Radiator to Case Radiator spots.
    /// </summary>
    /// <param name="case">the case input</param>
    /// <param name="cooler">the cooler input</param>
    /// <returns>Returns result True, if cooler[air(Height)/liquid(radiator)] can fit in the case.</returns>
    public bool CompareCaseCoolerType(Case @case, Cooler cooler)
    {
        if (cooler.IsAir)
        {
            if (@case?.MaxCpuAirCoolerHeight == null || cooler.Height == null) return false;

            return @case?.MaxCpuAirCoolerHeight >= cooler.Height;
        }
        else 
        {
            if (cooler.LiquidCoolerLengthMM == null || cooler.LiquidCoolerLengthMM.IsWhiteSpace()) return false;

            string requiredSize = cooler.LiquidCoolerLengthMM;
            bool isFitting = @case.MaxRadiatorSizeByLocation?.Values.Any(mrSize => mrSize.Contains(requiredSize)) ?? false;

            return isFitting;
        }
    }

    /// <summary>
    /// Compare sockets of cpu and motherboard
    /// </summary>
    /// <param name="cpu"></param>
    /// <param name="motherboard"></param>
    /// <returns></returns>
    public bool CompareCpuMotherboardSockets(CPU cpu, Motherboard motherboard)
    {
        if (cpu.Socket == null || motherboard.Socket == null) return false;

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
        if (gpu.PowerConsumption == null || psu?.Watts == null) return false;

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
        if (cpu.PowerConsumption == null || gpu.PowerConsumption == null ||
            motherboard.PowerConsumption == null || psu?.Watts == null)
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
        if (cpu.PowerConsumption == null || gpu.PowerConsumption == null ||
            motherboard.PowerConsumption == null || psu?.Watts == null || customAdditionalWatts == null)
            return false;

        double? allComponentsWatts = cpu.PowerConsumption + gpu.PowerConsumption + motherboard.PowerConsumption + customAdditionalWatts;

        return allComponentsWatts <= psu.Watts;
    }

    /// <summary>
    /// Compare RAM and motherboard RAM types, it ignores upper/down cases, dashes, spaces
    /// </summary>
    /// <returns>True if compatible, false - not</returns>
    public bool CompareRamMotherboardMemoryType(RAM ram, Motherboard motherboard)
    {
        if (ram.Type == null || motherboard.MemoryType == null) return false;

        return Normalize(ram.Type) == Normalize(motherboard.MemoryType);
    }

    /// <summary>
    /// Compares PCIe versions of GPU and motherboard and returns a compatibility score (0-100).
    /// </summary>
    /// <param name="gpu"></param>
    /// <param name="motherboard"></param>
    /// <returns>Returns score from 0(worst) to 100(best). Also returns 0 if any of the input objects are null.</returns>
    public double GetGpuMotherboardPcieInterfacesScore(GPU gpu, Motherboard motherboard)
    {
        if (gpu?.LanesNeeded == null || motherboard?.PcieLanes == null) return 0;

        double actualLanesUsed = Math.Min(gpu.LanesNeeded, motherboard.PcieLanes);

        if (gpu.LanesNeeded == 0) return 0;

        double laneScore = actualLanesUsed / gpu.LanesNeeded * 100;
        PCIeVersion bottleneckVersion = (PCIeVersion)Math.Min(gpu.PCIeVersion, motherboard.PCIEVersion);
        double bottleneckMultiplier = GetRelativeBandwidthMultiplier(bottleneckVersion);
        double gpuRequiredMultiplier = GetRelativeBandwidthMultiplier((PCIeVersion)gpu.PCIeVersion);
        double versionScore = (bottleneckMultiplier / gpuRequiredMultiplier) * 100;
        double finalScore = Math.Min(laneScore, versionScore);

        if (motherboard.PcieLanes < 4) return 0;

        return finalScore;
    }
    #endregion

    #region Helper functions
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
    #endregion
}
