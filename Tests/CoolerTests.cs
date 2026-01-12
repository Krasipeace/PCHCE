namespace Tests;

using Core;
using Core.Components;
using Core.Enums;

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
    public void CompareCaseCoolerType_CanFitInCase_ReturnsTrue()
    {
        var @case = new Case
        {
            MaxRadiatorSizeByLocation = new Dictionary<RadiatorLocation, IReadOnlyCollection<RadiatorSize>>
            {
                { RadiatorLocation.Top, new [] { RadiatorSize.R360, RadiatorSize.R280, RadiatorSize.R240 } },
                { RadiatorLocation.Front, new [] { RadiatorSize.R420, RadiatorSize.R360, RadiatorSize.R280 } },
                { RadiatorLocation.Rear, new [] { RadiatorSize.R120 } }
            }
        };

        var cooler = new Cooler
        {
            IsAir = false,
            RadiatorSize = RadiatorSize.R360
        };

        var cooler2 = new Cooler
        {
            IsAir = false,
            RadiatorSize = RadiatorSize.R280
        };

        var cooler3 = new Cooler
        {
            IsAir = false,
            RadiatorSize = RadiatorSize.R120
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
    public void CompareCaseCoolerType_CannotFit_ReturnsFalse()
    {
        var @case = new Case
        {
            MaxRadiatorSizeByLocation = new Dictionary<RadiatorLocation, IReadOnlyCollection<RadiatorSize>>
            {
                { RadiatorLocation.Top, new [] {RadiatorSize.R240, RadiatorSize.R280} }
            }
        };

        var cooler = new Cooler
        {
            IsAir = false,
            Length = 280
        };

        var cooler2 = new Cooler
        {
            IsAir = false,
            Length = 420
        };

        var cooler3 = new Cooler
        {
            IsAir = false,
            RadiatorSize = RadiatorSize.R140
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

    [Test]
    public void CompareCaseCoolerType_CanFit_ReturnsTrue()
    {
        var cooler = new Cooler
        {
            IsAir = true,
            Height = 150
        };

        var @case = new Case
        {
            MaxCpuAirCoolerHeight = 165
        };

        var result = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler);
        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareCaseCoolerType_CannotFit2_ReturnsFalse()
    {
        var @case = new Case
        {
            MaxCpuAirCoolerHeight = 150
        };

        var cooler = new Cooler
        {
            IsAir = true,
            Height = 160
        };

        var result = _compatibilityEvaluator.CompareCaseCoolerType(@case, cooler);
        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareCoolerRamTypeClearance_ReturnsFalse_WhenCoolerIsNull()
    {
        var ram = new RAM { Height = 40 };

        var result = _compatibilityEvaluator.CompareCoolerRamTypeClearance(null!, ram);

        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareCoolerRamTypeClearance_ReturnsFalse_WhenRamIsNull()
    {
        var cooler = new Cooler { IsAir = true, RamClearance = 45 };

        var result = _compatibilityEvaluator.CompareCoolerRamTypeClearance(cooler, null!);

        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareCoolerRamTypeClearance_ReturnsTrue_ForLiquidCooler_RegardlessOfRam()
    {
        var cooler = new Cooler
        {
            IsAir = false,
            RamClearance = null
        };

        var ram = new RAM
        {
            IsLowProfile = false,
            Height = 60
        };

        var result = _compatibilityEvaluator.CompareCoolerRamTypeClearance(cooler, ram);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareCoolerRamTypeClearance_ReturnsTrue_ForLowProfileRam_WithAirCooler()
    {
        var cooler = new Cooler
        {
            IsAir = true,
            RamClearance = 30
        };

        var ram = new RAM
        {
            IsLowProfile = true,
            Height = 50
        };

        var result = _compatibilityEvaluator.CompareCoolerRamTypeClearance(cooler, ram);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ReturnsTrue_WhenRamHeightEqualsClearance()
    {
        var cooler = new Cooler
        {
            IsAir = true,
            RamClearance = 42
        };

        var ram = new RAM
        {
            IsLowProfile = false,
            Height = 42
        };

        var result = _compatibilityEvaluator.CompareCoolerRamTypeClearance(cooler, ram);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareCoolerRamTypeClearance_ReturnsTrue_WhenRamHeightIsLessThanClearance()
    {
        var cooler = new Cooler
        {
            IsAir = true,
            RamClearance = 45
        };

        var ram = new RAM
        {
            IsLowProfile = false,
            Height = 40
        };

        var result = _compatibilityEvaluator.CompareCoolerRamTypeClearance(cooler, ram);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareCoolerRamTypeClearance_ReturnsFalse_WhenRamHeightExceedsClearance()
    {
        var cooler = new Cooler
        {
            IsAir = true,
            RamClearance = 40
        };

        var ram = new RAM
        {
            IsLowProfile = false,
            Height = 45
        };

        var result = _compatibilityEvaluator.CompareCoolerRamTypeClearance(cooler, ram);

        Assert.That(result, Is.False);
    }
    #endregion
}
