namespace Tests;

using Core;
using Core.Components;

public class CoolerTests
{
    private RankEvaluator _evaluator;

    [SetUp]
    public void Setup()
    {
        _evaluator = new RankEvaluator();
    }

    [Test]
    public void RankCooler_HighEnd_ReturnsAtLeast90()
    {
        var cooler = new Cooler
        {
            IsAir = false,
            RPM = 2500,
            CFM = 100
        };

        var result = _evaluator.RankCooler(cooler);
        Assert.That(result, Is.AtLeast(90));
    }

    [Test]
    public void RankCooler_MidRange_ReturnsAround50()
    {
        var cooler = new Cooler
        {
            IsAir = true,
            RPM = 2000,
            CFM = 67
        };

        var result = _evaluator.RankCooler(cooler);
        Assert.That(result, Is.InRange(40, 60));
    }

    [Test]
    public void RankCooler_LowEnd_ReturnsLowScore()
    {
        var cooler = new Cooler
        {
            IsAir = true,
            RPM = 1500,
            CFM = 44
        };

        var result = _evaluator.RankCooler(cooler);
        Assert.That(result, Is.LessThan(40));
    }

    [Test]
    public void RankCooler_ZeroSpecs_ReturnsZero()
    {
        var cooler = new Cooler{ IsAir = true };
        var result = _evaluator.RankCooler(cooler);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void RankCooler_ExceedsMaxSpecs_ClampsTo100()
    {
        var cooler = new Cooler
        {
            IsAir = false,
            RPM = 3000,
            CFM = 148
        };

        var result = _evaluator.RankCooler(cooler);
        Assert.That(result, Is.EqualTo(100));
    }
}
