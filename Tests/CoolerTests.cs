namespace Tests;

using Core;
using Core.Components;

public class CoolerTests
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
    public void RankCooler_HighEnd_ReturnsAtLeast90()
    {
        var cooler = new Cooler
        {
            IsAir = false,
            RPM = 2500,
            CFM = 100
        };

        var result = _rankEvaluator.RankCooler(cooler);
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

        var result = _rankEvaluator.RankCooler(cooler);
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

        var result = _rankEvaluator.RankCooler(cooler);
        Assert.That(result, Is.LessThan(40));
    }

    [Test]
    public void RankCooler_ZeroSpecs_ReturnsZero()
    {
        var cooler = new Cooler { IsAir = true };
        var result = _rankEvaluator.RankCooler(cooler);
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

        var result = _rankEvaluator.RankCooler(cooler);
        Assert.That(result, Is.EqualTo(100));
    }
    #endregion

    #region Compatibility Evaluator Tests
    [Test]
    public void EvalCaseLiquidCoolerCanFitInCase_ReturnsTrue()
    {
        var @case = new Case
        {
            MaxRadiatorSizeByLocation = new Dictionary<string, string>
            {
                { "Top", "360" },
                { "Front", "240" },
                { "Rear", "120" }
            }
        };

        var cooler = new Cooler
        {
            IsAir = false,
            LiquidCoolerLengthMM = "360"
        };

        var cooler2 = new Cooler
        {
            IsAir = false,
            LiquidCoolerLengthMM = "240"
        };

        var cooler3 = new Cooler
        {
            IsAir = false,
            LiquidCoolerLengthMM = "120"
        };

        var result = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler);
        var result2 = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler2);
        var result3 = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler3);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(result2, Is.True);
            Assert.That(result3, Is.True);
        });
    }

    [Test]
    public void EvalCaseLiquidCoolerCannotFit_ReturnsFalse()
    {
        var @case = new Case
        {
            MaxRadiatorSizeByLocation = new Dictionary<string, string>
            {
                { "Top", "360" },
                { "Front", "240" },
                { "Rear", "120" }
            }
        };

        var cooler = new Cooler
        {
            IsAir = false,
            LiquidCoolerLengthMM = "280"
        };

        var cooler2 = new Cooler
        {
            IsAir = false,
            LiquidCoolerLengthMM = "420"
        };

        var cooler3 = new Cooler
        {
            IsAir = false,
            LiquidCoolerLengthMM = "140"
        };

        var result = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler);
        var result2 = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler2);
        var result3 = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler3);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
            Assert.That(result3, Is.False);
        });
    }
    #endregion
}
