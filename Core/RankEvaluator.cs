namespace Core;

using Core.Components;

public class RankEvaluator
{
    /// <summary>
    /// Evaluating Storage Drive, depending on type, read/write speeds.
    /// </summary>
    /// <param name="storage"></param>
    /// <returns>Math Evaluation of storage characteristics(0[worst] to 100[best]).</returns>
    public double RankStorage(Storage storage)
    {
        double score = 0;

        if (storage.IsNVMe == true) score += 10;
        else if (storage.IsSSD == true) score += 5;
        else if (storage.IsHDD == true) score += 1;

        score += Math.Min(45, (storage.ReadSpeed / 15000.0) * 45);
        score += Math.Min(45, (storage.WriteSpeed / 15000.0) * 45);

        return Math.Min(100, Math.Max(0, score));
    }

    /// <summary>
    /// Evaluating cpu, depending on its phys/logical cores, base/turbo clock frequency, cache size.
    /// </summary>
    /// <param name="cpu">the ranked cpu</param>
    /// <returns>Math Evaluation score of cpu characteristics(0[worst] to 100[best]).</returns>
    public double RankCpu(CPU cpu)
    {
        double score = 0;
        score += Math.Min(30, (cpu.Cores / 16.0) * 30);
        score += Math.Min(10, (cpu.Threads / 32.0) * 10);
        score += Math.Min(20, (cpu.BaseClockGhz / 4.7) * 20);
        score += Math.Min(20, (cpu.TurboClockGhz / 5.7) * 20);
        score += Math.Min(20, (cpu.CacheSize / 64.0) * 20);

        return Math.Min(100, Math.Max(0, score));
    }

    /// <summary>
    /// Evaluating gpu, depending on its graphical cores, memory capacity, frequency, physical interface.
    /// </summary>
    /// <param name="gpu">the ranked gpu</param>
    /// <returns>Math Evaluation score of gpu characteristics(0[worst] to 100[best]).</returns>
    public double RankGpu(GPU gpu)
    {
        double score = 0;
        if (gpu.HasCuda == true) score += 5;

        score += Math.Min(30, (gpu.Cores / 21760.0) * 30);
        score += Math.Min(30, (gpu.Capacity / 32.0) * 30);
        score += Math.Min(30, (gpu.Frequency / 3500.0) * 30);
        score += Math.Min(5, (gpu.Interface / 384.0) * 5);

        return Math.Min(100, Math.Max(0, score));
    }

    /// <summary>
    /// Evaluating ram, depending on speed, voltage, latency and xmp/expo settings.
    /// </summary>
    /// <param name="ram">the ranked ram</param>
    /// <returns>Math Evaluation score of ram characteristics(0[worst] to 100[best]).</returns>
    public double RankRam(RAM ram)
    {
        double score = 0;
        score += Math.Min(40, (ram.Speed / 12000.0) * 40);
        score += Math.Min(10, (ram.Voltage / 1.5) * 10);

        if (ram.CASLatency > 0)
        {
            double clScore = Math.Max(10, 40 - ram.CASLatency);
            score += Math.Min(25, clScore);
        }

        if (ram.HasXMPorExpo) score += 25;

        return Math.Min(100, Math.Max(0, score));
    }

    /// <summary>
    /// Evaluating Motherboard, depending on pci-e version and max memory capacity
    /// </summary>
    /// <param name="motherboard">the ranked motherboard</param>
    /// <returns>Math Evaluation score of motherboard characteristics(0[worst] to 100[best]).</returns>
    public double RankMotherboard(Motherboard motherboard)
    {
        double score = 0;
        double versionScore = (int)motherboard.PCIEVersion;
        score += Math.Min(80, (versionScore / 5.0) * 80);
        score += Math.Min(20, (motherboard.MaxMemoryCapacity / 512.0) * 20);

        return Math.Min(100, Math.Max(0, score));
    }

    /// <summary>
    /// Evaluating PSU, depending on efficiency and overrall watts
    /// </summary>
    /// <param name="psu">the ranked psu</param>
    /// <returns>Math Evaluation score of psu characteristics(0[worst] to 100[best]).</returns>
    public double RankPsu(PowerSupply psu)
    {
        double score = 0;
        score += Math.Min(30, (psu.Watts / 1600.0) * 30);
        score += Math.Min(70, (psu.Efficiency / 100.0) * 70);

        return Math.Min(100, Math.Max(0, score));
    }

    /// <summary>
    /// Evaluating Cooler, depending on type, RPM, CFM
    /// </summary>
    /// <param name="cooler">the ranked cooler</param>
    /// <returns>Math Evaluation score of cooler characteristics(0[worst] to 100[best]).</returns>
    public double RankCooler(Cooler cooler)
    {
        double score = 0;
        if (!cooler.IsAir) score += 30;

        score += Math.Min(20, (cooler.RPM / 3000.0) * 20);
        score += Math.Min(50, (cooler.CFM / 100.0) * 50);

        return Math.Min(100, Math.Max(0, score));
    }

    /// <summary>
    /// Evaluating Fan, depending on RPM and CFM
    /// </summary>
    /// <param name="fan">the ranked fan</param>
    /// <returns>Math Evaluation score of fan characteristics(0[worst] to 100[best]).</returns>
    public double RankFan(Fan fan)
    {
        double score = 0;
        score += Math.Min(30, (fan.RPM / 3000.0) * 30);
        score += Math.Min(70, (fan.CFM / 100.0) * 70);

        return Math.Min(100, Math.Max(0, score));
    }
}
