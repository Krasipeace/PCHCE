namespace Tests;

using Core;
using Core.Components;

public class PsuTests
{
    private RankEvaluator _evaluator;

    [SetUp]
    public void Setup()
    {
        _evaluator = new RankEvaluator();
    }

    [Test]
    public void RankPsu_HighSpecs_ReturnsAtLeast80()
    {
        var psu = new PowerSupply
        {
            Watts = 1600,
            Efficiency = 90
        };

        var result = _evaluator.RankPsu(psu);
        Assert.That(result, Is.AtLeast(80));
    }

    [Test]
    public void RankPsu_ZeroSpecs_ReturnsZero()
    {
        var psu = new PowerSupply();
        var result = _evaluator.RankPsu(psu);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void RankPsu_ExceedsMaxSpecs_ClampsTo100()
    {
        var psu = new PowerSupply
        {
            Watts = 2000,
            Efficiency = 100
        };

        var result = _evaluator.RankPsu(psu);
        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void RankGoodPsuEfficiency_ReturnsAtLeast50()
    {
        var psu = new PowerSupply
        {
            Watts = 550,
            Efficiency = 90
        };

        var result = _evaluator.RankPsu(psu);
        Assert.That(result, Is.AtLeast(50));
    }

    [Test]
    public void RankGoodPsu_Watts_ReturnsAtLeast50()
    {
        var psu = new PowerSupply
        {
            Watts = 1050,
            Efficiency = 80
        };

        var result = _evaluator.RankPsu(psu);
        Assert.That(result, Is.AtLeast(50));
    }
}