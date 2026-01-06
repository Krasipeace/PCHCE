namespace Tests;

using Core;
using Core.Components;

public class GpuTests
{
    private RankEvaluator _rankEvaluator;
    private CompatibilityEvaluator _compatibilityEvaluator;

    [SetUp]
    public void Setup()
    {
        _rankEvaluator = new RankEvaluator();
        _compatibilityEvaluator = new CompatibilityEvaluator();
    }

    #region RankEvaluator Tests
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

        var result = _rankEvaluator.RankGpu(gpu);

        Assert.That(result, Is.AtLeast(80));
    }

    [Test]
    public void RankGpu_ZeroSpecs_ReturnsZero()
    {
        var gpu = new GPU();

        var result = _rankEvaluator.RankGpu(gpu);

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

        var result = _rankEvaluator.RankGpu(gpu);

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

        var result = _rankEvaluator.RankGpu(gpu);

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

        var result = _rankEvaluator.RankGpu(gpu);

        Assert.That(result, Is.GreaterThan(66));
    }
    #endregion

    #region Compatibility Evaluator Tests
    [Test]
    public void CompareCaseGpuLength_CanFit_ReturnsTrue()
    {
        var @case = new Case
        {
            MaxGpuLength = 420
        };

        var gpu = new GPU
        {
            Length = 335
        };

        var result = _compatibilityEvaluator.CompareCaseGpuLength(@case, gpu);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareCaseGpuLength_CannotFit_ReturnsFalse()
    {
        var @case = new Case
        {
            MaxGpuLength = 320
        };

        var gpu = new GPU
        {
            Length = 381
        };

        var result = _compatibilityEvaluator.CompareCaseGpuLength(@case, gpu);

        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareCaseGpuLength_HasNullInputOrZeroes_ReturnsFalse()
    {
        var caseNull = new Case();
        var gpuNull = new GPU();
        var @case = new Case
        {
            MaxGpuLength = 320
        };
        var gpu = new GPU
        {
            Length = 280
        };
        var caseZero = new Case
        {
            MaxGpuLength = 0
        };
        var gpuZero = new GPU
        {
            Length = 0
        };

        var result = _compatibilityEvaluator.CompareCaseGpuLength(caseNull, gpu);
        var result2 = _compatibilityEvaluator.CompareCaseGpuLength(@case, gpuNull);
        var result3 = _compatibilityEvaluator.CompareCaseGpuLength(caseNull, gpuNull);
        var result4 = _compatibilityEvaluator.CompareCaseGpuLength(caseZero, gpu);
        var result5 = _compatibilityEvaluator.CompareCaseGpuLength(@case, gpuZero);
        var result6 = _compatibilityEvaluator.CompareCaseGpuLength(caseZero, gpuNull);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
            Assert.That(result3, Is.False);
            Assert.That(result4, Is.False);
            Assert.That(result5, Is.False);
            Assert.That(result6, Is.False);
        });
    }

    [Test]
    public void CompareCaseGpuLength_CanFitWithMMvalues_ReturnsTrue()
    {
        double caseMaxGpuLength = 350;
        double gpuLength = 330;

        var result = _compatibilityEvaluator.CompareCaseGpuLength(caseMaxGpuLength, gpuLength);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareCaseGpuLength_CannotFitWithMMvalues_ReturnsFalse()
    {
        double caseMaxGpuLength = 300;
        double gpuLength = 330;

        var result = _compatibilityEvaluator.CompareCaseGpuLength(caseMaxGpuLength, gpuLength);

        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareCaseGpuLength_WeirdCasesWithMMValues_ReturnsFalse()
    {
        double caseMaxGpuLength = 300;
        double gpuLength = -301;

        var result = _compatibilityEvaluator.CompareCaseGpuLength(caseMaxGpuLength, gpuLength);
        var result2 = _compatibilityEvaluator.CompareCaseGpuLength(caseMaxGpuLength - 300, gpuLength);
        var result3 = _compatibilityEvaluator.CompareCaseGpuLength(caseMaxGpuLength, gpuLength + 301);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
            Assert.That(result3, Is.False);
        });
    }
    #endregion
}