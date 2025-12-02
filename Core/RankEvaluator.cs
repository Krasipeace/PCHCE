namespace Core;

using Core.Components;

public class RankEvaluator
{
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
}

    public double RankCpu(CPU cpu)
    {
        double score = 0;
        score += Math.Min(30, (cpu.Cores / 16.0) * 30);
        score += Math.Min(10, (cpu.Threads / 32.0) * 10);
        score += Math.Min(20, (cpu.BaseClockGhz / 4.7) * 20);
        score += Math.Min(20, (cpu.TurboClockGhz / 6.0) * 20);
        score += Math.Min(20, (cpu.CacheSize / 64.0) * 20);

        return Math.Min(100, Math.Max(0, score));
    }

    public double RankGpu(GPU gpu)
    {
        double score = 0;
        if (gpu.HasCuda == true) score += 5;

        score += Math.Min(30, (gpu.Cores / 21760.0) * 30);
        score += Math.Min(30, (gpu.Capacity / 32.0) * 30);
        score += Math.Min(30, (gpu.Frequency / 3500.0) * 30);
        score += Math.Min(5, (gpu.Interface / 512.0) * 5);

        return Math.Min(100, Math.Max(0, score));
    }

    public double RankRam(RAM ram)
    {
        double score = 0;
        score += Math.Min(40, (ram.Speed / 12000.0) * 40);
        score += Math.Min(10, (ram.Voltage / 1.5) * 10);

        if (ram.CASLatency > 0)
        {
            double clScore = Math.Max(0, 40 - ram.CASLatency);
            score += Math.Min(25, clScore);
        }

        if (ram.HasXMPorExpo == true) score += 25;

        return Math.Min(100, Math.Max(0, score));
    }

    public double RankMotherboard(Motherboard motherboard)
    {
        double score = 0;
        score += Math.Min(80, (motherboard.PCIEVersion / 5.0) * 80);
        score += Math.Min(20, (motherboard.MaxMemoryCapacity / 512.0) * 20);

        return Math.Min(100, Math.Max(0, score));
    }

    public double RankPsu(PowerSupply psu)
    {
        double score = 0;
        score += Math.Min(30, (psu.Watts / 1600.0) * 30);
        score += Math.Min(70, (psu.Efficiency / 100.0) * 70);

        return Math.Min(100, Math.Max(0, score));
    }

    public double RankCooler(Cooler cooler)
    {
        double score = 0;
        if (cooler.IsAir == false) score += 30;

        score += Math.Min(20, (cooler.RPM / 3000.0) * 20);
        score += Math.Min(50, (cooler.CFM / 100.0) * 50);

        return Math.Min(100, Math.Max(0, score));
    }

    public double RankFan(Fan fan)
    {
        double score = 0;
        score += Math.Min(30, (fan.RPM / 3000.0) * 30);
        score += Math.Min(70, (fan.CFM / 100.0) * 70);

        return Math.Min(100, Math.Max(0, score));
    }
}
