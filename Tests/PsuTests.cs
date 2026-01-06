namespace Tests;

using Core;
using Core.Components;
using Core.Enums;

public class PsuTests
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
    public void RankPsu_HighSpecs_ReturnsAtLeast80()
    {
        var psu = new PowerSupply
        {
            Watts = 1600,
            Efficiency = 90
        };

        var result = _rankEvaluator.RankPsu(psu);
        Assert.That(result, Is.AtLeast(80));
    }

    [Test]
    public void RankPsu_ZeroSpecs_ReturnsZero()
    {
        var psu = new PowerSupply();
        var result = _rankEvaluator.RankPsu(psu);
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

        var result = _rankEvaluator.RankPsu(psu);
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

        var result = _rankEvaluator.RankPsu(psu);
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

        var result = _rankEvaluator.RankPsu(psu);
        Assert.That(result, Is.AtLeast(50));
    }
    #endregion

    #region CompatibilityEvaluator Tests
    [Test]
    public void CompareGpuPsuWattsConsumption_WhenIsEnough_ReturnsTrue()
    {
        var psu = new PowerSupply
        {
            Watts = 650
        };

        var gpu = new GPU
        {
            PowerConsumption = 450
        };

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpu, psu);
        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareGpuPsuWattsPowerConsumption_WhenNotEnough_ReturnsFalse()
    {
        var psu = new PowerSupply
        {
            Watts = 650
        };

        var gpu = new GPU
        {
            PowerConsumption = 750
        };

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpu, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareGpuPsuWattsPowerConsumption_WhenAllInputIsNull_ReturnsFalse()
    {
        var psu = new PowerSupply();
        var gpu = new GPU();

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpu, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareGpuPsuWattsPowerConsumption_WhenOneInputIsNull_ReturnsFalse()
    {
        var psu = new PowerSupply();
        var gpu = new GPU
        {
            PowerConsumption = 550
        };

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpu, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareGpuPsuWattsPowerConsumption_WhenPsuWattsAreEnough_ReturnsTrue()
    {
        double gpuWatts = 650;
        double psuWatts = 850;

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpuWatts, psuWatts);

        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareGpuPsuWattsPowerConsumption_WhenPsuWattsAreNotEnough_ReturnsFalse()
    {
        double gpuWatts = 650;
        double psuWatts = 550;

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpuWatts, psuWatts);

        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareGpuPsuWattsPowerConsumption_WhenPsuWattsAreZeroOrNegative_ReturnsFalse()
    {
        double gpuWattsZero = 0.0;
        double psuWattsZero = 0.0;
        double gpuWattsNegative = -100;
        double psuWattsNegativeZero = -99;

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpuWattsNegative, psuWattsZero);
        var result2 = _compatibilityEvaluator.CompareGpuPsuWatts(gpuWattsZero, psuWattsNegativeZero);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
        });
    }

    [Test]
    public void CompareCasePsuFormFactor_WhenFactorIsInTheCollection_ReturnsTrue()
    {
        var psu = new PowerSupply
        {
            FormFactor = PsuFormFactor.ATX
        };

        var @case = new Case
        {
            SupportedPsuFormFactors =
            [
                PsuFormFactor.ATX,
                PsuFormFactor.SFX
            ]
        };

        var result = _compatibilityEvaluator.CompareCasePsuFormFactor(@case, psu);
        Assert.That(result, Is.True);
    }

    [Test]
    public void CompareCasePsuFormFactor_WhenFactorIsNotInTheCollection_ReturnsFalse()
    {
        var psu = new PowerSupply
        {
            FormFactor = PsuFormFactor.SFX
        };

        var @case = new Case
        {
            SupportedPsuFormFactors =
            [
                PsuFormFactor.ATX
            ]
        };

        var result = _compatibilityEvaluator.CompareCasePsuFormFactor(@case, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareCasePsuFormFactorFormFactor_WhenAllInputIsNull_ReturnsFalse()
    {
        var psu = new PowerSupply();
        var @case = new Case();

        var result = _compatibilityEvaluator.CompareCasePsuFormFactor(@case, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void CompareAllToPsuWattsComponents_WhenCompatible_ReturnsTrue()
    {
        var psu = new PowerSupply
        {
            Watts = 550
        };

        var cpu = new CPU
        {
            Name = "AMD Ryzen 7 - 5800X",
            PowerConsumption = 105
        };

        var gpu = new GPU
        {
            Name = "AMD RX 6600",
            PowerConsumption = 132
        };

        var motherboard = new Motherboard
        {
            Name = "AMD Asus B550 TUF",
            PowerConsumption = 70
        };

        double? customAdditionalWatts = 150;

        var result = _compatibilityEvaluator.CompareAllToPsuWatts(cpu, gpu, motherboard, psu);
        var result2 = _compatibilityEvaluator.CompareAllToPsuWatts(cpu, gpu, motherboard, psu, customAdditionalWatts);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(result2, Is.True);
        });
    }

    [Test]
    public void CompareAllToPsuWattsComponents_WhenInCompatible_ReturnsFalse()
    {
        var psu = new PowerSupply
        {
            Watts = 600
        };

        var cpu = new CPU
        {
            Name = "AMD Ryzen 7 - 5800X",
            PowerConsumption = 105
        };

        var gpu = new GPU
        {
            Name = "AMD RX 6900XT",
            PowerConsumption = 360
        };

        var motherboard = new Motherboard
        {
            Name = "AMD Asus B550 TUF",
            PowerConsumption = 70
        };

        double? customAdditionalWatts = 150;

        var result = _compatibilityEvaluator.CompareAllToPsuWatts(cpu, gpu, motherboard, psu);
        var result2 = _compatibilityEvaluator.CompareAllToPsuWatts(cpu, gpu, motherboard, psu, customAdditionalWatts);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
        });
    }

    [Test]
    public void CompareAllToPsuWattsComponents_WhenInputIsNull_ReturnsFalse()
    {
        var psu = new PowerSupply();

        var cpu = new CPU
        {
            PowerConsumption = 105
        };

        var gpu = new GPU
        {
            PowerConsumption = 360
        };

        var motherboard = new Motherboard
        {
            PowerConsumption = 70
        };

        double? customAdditionalWatts = 150;

        var result = _compatibilityEvaluator.CompareAllToPsuWatts(cpu, gpu, motherboard, psu);
        var result2 = _compatibilityEvaluator.CompareAllToPsuWatts(cpu, gpu, motherboard, psu, customAdditionalWatts);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
        });
    }
    #endregion
}