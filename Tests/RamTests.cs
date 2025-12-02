namespace Tests;

using Core;
using Core.Components;

public class RamTests
{
    private RankEvaluator _evaluator;

    [SetUp]
    public void Setup()
    {
        _evaluator = new RankEvaluator();
    }

    [Test]
    public void RankRam_HighSpecs_ReturnsAtLeast80()
    {
        var ram = new RAM
        {
            Speed = 12000,
            Voltage = 1.5,
            CASLatency = 12,
            HasXMPorExpo = true
        };

        var result = _evaluator.RankRam(ram);
        Assert.That(result, Is.AtLeast(80));
    }

    [Test]
    public void RankRam_ZeroSpecs_ReturnsZero()
    {
        var ram = new RAM();
        var result = _evaluator.RankRam(ram);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void RankRam_ExceedsMaxSpecs_ClampsTo100()
    {
        var ram = new RAM
        {
            Speed = 16000,
            Voltage = 2.0,
            CASLatency = 10,
            HasXMPorExpo = true
        };

        var result = _evaluator.RankRam(ram);
        Assert.That(result, Is.EqualTo(100));
    }
}