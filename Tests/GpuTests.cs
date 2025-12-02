namespace Tests;

using Core;
using Core.Components;

public class GpuTests
{
    private RankEvaluator _evaluator;

    [SetUp]
    public void Setup()
    {
        _evaluator = new RankEvaluator();
    }

    [Test]
    public void RankGpu_HighSpecs_ReturnsAtLeast80()
    {
        var gpu = new GPU
        {
            HasCuda = true,
            Cores = 21760,
            Capacity = 32,
            Frequency = 3500,
            Interface = 512
        };

        var result = _evaluator.RankGpu(gpu);
        Assert.That(result, Is.AtLeast(80));
    }

    [Test]
    public void RankGpu_ZeroSpecs_ReturnsZero()
    {
        var gpu = new GPU();
        var result = _evaluator.RankGpu(gpu);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void RankGpu_ExceedsMaxSpecs_ClampsTo100()
    {
        var gpu = new GPU
        {
            HasCuda = true,
            Cores = 21760,
            Capacity = 32,
            Frequency = 3500,
            Interface = 512
        };

        var result = _evaluator.RankGpu(gpu);
        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void RankGpu_HasNotCudaAndIsVeryHighRank_ReturnsAtLeast95()
    {
        var gpu = new GPU
        {
            HasCuda = false,
            Cores = 21760,
            Capacity = 32,
            Frequency = 3500,
            Interface = 384
        };

        var result = _evaluator.RankGpu(gpu);
        Assert.That(result, Is.AtLeast(95));
    }

    [Test]
    public void RankGpu_HasNotCudaAndMediumRank()
    {
        var gpu = new GPU
        {
            HasCuda = false,
            Cores = 15000,
            Capacity = 24,
            Frequency = 2500,
            Interface = 384
        };

        var result = _evaluator.RankGpu(gpu);
        Assert.That(result, Is.GreaterThan(66));
    }
}