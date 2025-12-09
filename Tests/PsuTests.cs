namespace Tests;

using Core;
using Core.Components;

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
    public void EvalGpuPsuPowerConsumption_WhenIsEnough_ReturnsTrue()
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
    public void EvalGpuPsuPowerConsumption_WhenNotEnough_ReturnsFalse()
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
    public void EvalGpuPsuPowerConsumption_WhenAllInputIsNull_ReturnsFalse()
    {
        var psu = new PowerSupply();
        var gpu = new GPU();

        var result = _compatibilityEvaluator.CompareGpuPsuWatts(gpu, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalGpuPsuPowerConsumption_WhenOneInputIsNull_ReturnsFalse()
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
    public void EvalPsuCaseFormFactor_WhenFactorIsInTheCollection_ReturnsTrue()
    {
        var psu = new PowerSupply
        {
            FormFactor = "atx"
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
    public void EvalPsuCaseFormFactor_WhenFactorIsNotInTheCollection_ReturnsFalse()
    {
        var psu = new PowerSupply
        {
            FormFactor = "sfx"
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
    public void EvalPsuCaseFormFactor_WhenAllInputIsNull_ReturnsFalse()
    {
        var psu = new PowerSupply();
        var @case = new Case();

        var result = _compatibilityEvaluator.CompareCasePsuFormFactor(@case, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalPsuCaseFormFactor_WhenOneInputIsNull_ReturnsFalse()
    {
        var psu = new PowerSupply();
        var @case = new Case
        {
            SupportedPsuFormFactors =
            [
                PsuFormFactor.ATX,
                PsuFormFactor.FlexATX,
                PsuFormFactor.SFX_L,
                PsuFormFactor.TFX,
                PsuFormFactor.SFX
            ]
        };

        var result = _compatibilityEvaluator.CompareCasePsuFormFactor(@case, psu);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalAllComponentsPsuWatts_WhenCompatible_ReturnsTrue()
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
    public void EvalAllComponentsPsuWatts_WhenInCompatible_ReturnsFalse()
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
    public void EvalAllComponentsPsuWatts_WhenInputIsNull_ReturnsFalse()
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