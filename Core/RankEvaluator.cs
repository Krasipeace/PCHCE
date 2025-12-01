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
