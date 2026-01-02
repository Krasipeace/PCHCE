namespace Core;

using Core.Components;
using Core.Enums;

public class CompatibilityEvaluator
{
    #region Compare component characteristics
    /// <summary>
    /// Compare Case form factor and motherboard form factor from objects
    /// </summary>
    /// <param name="case">case object</param>
    /// <param name="motherboard">motherboard object</param>
    /// <returns>Returns True, if motherboard form factor is in case's supported motherboards list</returns>
    public bool CompareCaseMotherBoardFormFactor(Case @case, Motherboard motherboard)
    {
        if (@case?.SupportedMbFormFactors == null || motherboard?.FormFactor == null) return false;

        return @case.SupportedMbFormFactors.Contains(motherboard.FormFactor);
    }

    /// <summary>
    /// Compare Case's form factors and motherboard form factor as strings
    /// </summary>
    /// <param name="caseMbFormFactors">case form factor's array</param>
    /// <param name="motherboardFormFactor">motherboard form factor</param>
    /// <returns>Returns True, if motherboard form factor is in case's supported motherboards form factors list</returns>
    public bool CompareCaseMotherBoardFormFactor(string[] caseMbFormFactors, string motherboardFormFactor)
    {
        if (caseMbFormFactors.Length == 0 || motherboardFormFactor is null) return false;

        var normalizedMbFormfactors = Normalize(caseMbFormFactors);
        var normalizedMbFormfactor = Normalize(motherboardFormFactor);

        return normalizedMbFormfactors.Contains(normalizedMbFormfactor);
    }

    /// <summary>
    /// Compare PC case power supply form factors to power supply form factor.
    /// </summary>
    /// <param name="case">the pc case input.</param>
    /// <param name="psu">the power supply unit input.</param>
    /// <returns></returns>
    public bool CompareCasePsuFormFactor(Case @case, PowerSupply psu) => @case.SupportedPsuFormFactors.Contains(psu.FormFactor);

    /// <summary>
    /// Check Max PC Case Gpu Length to actual GPU Length
    /// </summary>
    /// <param name="case">the pc case input</param>
    /// <param name="gpu">the gpu input</param>
    /// <returns>Returns True, if GPU can fit the case, False - otherwise</returns>
    public bool CompareCaseGpuLength(Case @case, GPU gpu)
    {
        if (@case?.MaxGpuLength == null || gpu.Length == null || @case.MaxGpuLength == 0 || gpu.Length == 0) return false;

        return @case.MaxGpuLength >= gpu.Length;
    }

    /// <summary>
    /// Check Max PC Case Gpu Length to actual GPU Length
    /// </summary>
    /// <param name="caseMaxGpuLengthmm">PC Case's GPU max possible Length in mm</param>
    /// <param name="gpuLengthmm">GPU Length in mm</param>
    /// <returns>Returns True, if GPU can fit the case, False - otherwise</returns>
    public bool CompareCaseGpuLength(double caseMaxGpuLengthmm, double gpuLengthmm)
    {
        if (caseMaxGpuLengthmm <= 0 || gpuLengthmm <= 0) return false;

        return caseMaxGpuLengthmm >= gpuLengthmm;
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
            if (cooler.Height is null || cooler.Height <= 0 || 
                @case?.MaxCpuAirCoolerHeight is null || @case.MaxCpuAirCoolerHeight == 0) 
                return false;

            return cooler.Height <= @case.MaxCpuAirCoolerHeight;
        }
        else
        {
            if (cooler.RadiatorSize is null) return false;

            return @case.MaxRadiatorSizeByLocation.Values.Any(sizes => sizes.Contains(cooler.RadiatorSize.Value));
        } 
    }

    /// <summary>
    /// Compare sockets of cpu and motherboard
    /// </summary>
    /// <param name="cpu">CPU input object</param>
    /// <param name="motherboard">Motherboard input object</param>
    /// <returns>Returns True, if sockets match, False - otherwise.</returns>
    public bool CompareCpuMotherboardSockets(CPU cpu, Motherboard motherboard)
    {
        if (cpu.Socket == null || motherboard.Socket == null) return false;

        return Normalize(cpu.Socket) == Normalize(motherboard.Socket);
    }

    /// <summary>
    /// Compare sockets of cpu and motherboard as strings
    /// </summary>
    /// <param name="cpuSocket">CPU socket as string. Example: "AM5"</param>
    /// <param name="motherboardSocket">Motherboard socket as string. Example: "AM5"</param>
    /// <returns>Returns True, if sockets match, False - otherwise.</returns>
    public bool CompareCpuMotherboardSockets(string cpuSocket, string motherboardSocket) => 
        (!string.IsNullOrEmpty(cpuSocket) || !string.IsNullOrEmpty(motherboardSocket)) && 
        Normalize(cpuSocket) == Normalize(motherboardSocket);

    /// <summary>
    /// Compare GPU power consumption to pc system PSU
    /// </summary>
    /// <param name="gpu">gpu power consumption</param>
    /// <param name="psu">power supply wattage</param>
    /// <returns>True, if Gpu watts are less or equal to PSU watts.</returns>
    public bool CompareGpuPsuWatts(GPU gpu, PowerSupply psu)
    {
        if (gpu.PowerConsumption == null || psu?.Watts == null) return false;

        return gpu.PowerConsumption <= psu.Watts;
    }

    /// <summary>
    /// Compare GPU power consumption to pc system PSU
    /// </summary>
    /// <param name="gpuWatts">GPU wattage value</param>
    /// <param name="psuWatts">PSU wattage value</param>
    /// <returns>True, if GPU watts are less or equal of PSU watts</returns>
    public bool CompareGpuPsuWatts(double gpuWatts, double psuWatts) => 
        gpuWatts <= 0 || psuWatts <= 0 ? false : gpuWatts <= psuWatts;

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
    public bool CompareRamMotherboardMemoryType(RAM ram, Motherboard motherboard) => ram.Type == motherboard.MemoryType;

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
        double gpuScore = (int)gpu.PCIeVersion;
        double motherboardScore = (int)motherboard.PCIEVersion;
        PCIeVersion bottleneckVersion = (PCIeVersion)Math.Min(gpuScore, motherboardScore);
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
    /// Check string[] for upper/down/spaces/dashes and ignores them
    /// </summary>
    /// <param name="s">the string to check</param>
    /// <returns>Returns new string to uppercase without spaces/dashes</returns>
    private static string[] Normalize(string[] values) => values?.Select(Normalize).ToArray() ?? [];

    /// <summary>
    /// Check string for upper/down/spaces/dashes and ignores them
    /// </summary>
    /// <param name="s">the string to check</param>
    /// <returns>Returns new string to uppercase without spaces/dashes</returns>
    private static string Normalize(string? sValue) => sValue is null ? string.Empty : Normalize(sValue.AsSpan());

    /// <summary>
    /// Check chars for upper/down/spaces/dashes and ignores them
    /// </summary>
    /// <param name="s">the string to check</param>
    /// <returns>Returns new string to uppercase without spaces/dashes</returns>
    private static string Normalize(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty) return string.Empty;

        Span<char> buffer = stackalloc char[input.Length];
        int index = 0;

        foreach (var c in input)
            if (!char.IsWhiteSpace(c) && c != '-')
                buffer[index++] = char.ToUpperInvariant(c);

        return new string(buffer[..index]);
    }
    #endregion
}
