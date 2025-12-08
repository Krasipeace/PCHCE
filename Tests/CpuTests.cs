namespace Tests;

using Core;
using Core.Components;

public class CpuTests
{
    private RankEvaluator _rankEvaluator;

    [SetUp]
    public void Setup()
    {
        _rankEvaluator = new RankEvaluator();
    }

    #region RankEvaluator Tests
    [Test]
    public void RankCpu_HighEnd_ReturnsAtLeast90()
    {
        var cpu = new CPU
        {
            Cores = 16,
            Threads = 32,
            BaseClockGhz = 4.7,
            TurboClockGhz = 5.7,
            CacheSize = 64
        };

        var result = _rankEvaluator.RankCpu(cpu);
        Assert.That(result, Is.AtLeast(90));
    }

    [Test]
    public void RankCpu_MidRange_ReturnsAround50()
    {
        var cpu = new CPU
        {
            Cores = 8,
            Threads = 16,
            BaseClockGhz = 3.5,
            TurboClockGhz = 4.0,
            CacheSize = 32
        };

        var result = _rankEvaluator.RankCpu(cpu);
        Assert.That(result, Is.InRange(40, 60));
    }

    [Test]
    public void RankCpu_LowEnd_ReturnsLowScore()
    {
        var cpu = new CPU
        {
            Cores = 4,
            Threads = 4,
            BaseClockGhz = 2.0,
            TurboClockGhz = 2.5,
            CacheSize = 8
        };

        var result = _rankEvaluator.RankCpu(cpu);
        Assert.That(result, Is.LessThan(40));
    }

    [Test]
    public void RankCpu_ZeroSpecs_ReturnsZero()
    {
        var cpu = new CPU();
        var result = _rankEvaluator.RankCpu(cpu);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void RankCpu_ExceedsMaxSpecs_ClampsTo100()
    {
        var cpu = new CPU
        {
            Cores = 32,
            Threads = 64,
            BaseClockGhz = 5.0,
            TurboClockGhz = 7.0,
            CacheSize = 128
        };

        var result = _rankEvaluator.RankCpu(cpu);
        Assert.That(result, Is.EqualTo(100));
    }
    #endregion
}