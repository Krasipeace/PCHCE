namespace Tests;

using Core;
using Core.Components;

public class MotherboardTests
{
    private RankEvaluator _rankEvaluator;

    [SetUp]
    public void Setup()
    {
        _rankEvaluator = new RankEvaluator();
    }

    [Test]
    public void RankMotherboard_HighSpecs_ReturnsAtLeast80()
    {
        var motherboard = new Motherboard
        {
            PCIEVersion = 5,
            MaxMemoryCapacity = 512
        };

        var result = _rankEvaluator.RankMotherboard(motherboard);
        Assert.That(result, Is.AtLeast(80));
    }

    [Test]
    public void RankMotherboard_ZeroSpecs_ReturnsZero()
    {
        var motherboard = new Motherboard();
        var result = _rankEvaluator.RankMotherboard(motherboard);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void RankMotherboard_ExceedsMaxSpecs_ClampsTo100()
    {
        var motherboard = new Motherboard
        {
            PCIEVersion = 6,
            MaxMemoryCapacity = 1024
        };

        var result = _rankEvaluator.RankMotherboard(motherboard);
        Assert.That(result, Is.EqualTo(100));
    }
}