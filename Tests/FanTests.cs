namespace Tests;

using Core;
using Core.Components;

public class FanTests
{
    private RankEvaluator _evaluator;

    [SetUp]
    public void Setup()
    {
        _evaluator = new RankEvaluator();
    }

    [Test]
    public void RankFan_HighSpecs_ReturnsAtLeast90()
    {
        var fan = new Fan
        {
            CFM = 100,
            RPM = 2500
        };

        var result = _evaluator.RankFan(fan);
        Assert.That(result, Is.AtLeast(90));
    }

    [Test]
    public void RankFan_ExceedsMaxSpecsReturns100()
    {
        var fan = new Fan
        {
            CFM = 150,
            RPM = 10000
        };

        var result = _evaluator.RankFan(fan);
        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void RankFan_NoSpecs_ReturnsZero()
    {
        var fan = new Fan();

        var result = _evaluator.RankFan(fan);
        Assert.That(result, Is.EqualTo(0));
    }
}