namespace Tests;

using Core;
using Core.Components;

public class MotherboardTests
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

    [Test]
    public void RankMotherboard_Null_ReturnsZero()
    {
        var motherboard = new Motherboard();
        var result = _rankEvaluator.RankMotherboard(motherboard);
        Assert.That(result, Is.EqualTo(0));
    }
    #endregion

    #region CompatibilityEvaluator Tests
    [Test]
    public void EvalMotherboardRam_GivesTrueResult_WhenTypesAreTheSame()
    {
        var motherboard = new Motherboard
        {
            MemoryType = "DDR5"
        };

        var ram = new RAM
        {
            Type = "DDR5"
        };

        var result = _compatibilityEvaluator.CompareRamMotherboardMemoryType(ram, motherboard);
        Assert.That(result, Is.True);
    }

    [Test]
    public void EvalMotherBoardRam_GivesFalseResult_WhenTypesAreNotTheSame()
    {
        var motherboard = new Motherboard
        {
            MemoryType = "DDR5"
        };

        var ram = new RAM
        {
            Type = "DDR4"
        };

        var result = _compatibilityEvaluator.CompareRamMotherboardMemoryType(ram, motherboard);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalMotherBoardRam_GivesTrueResult_WhenNormalizeIsInAction()
    {
        var motherboard = new Motherboard
        {
            MemoryType = "DDR-5"
        };

        var ram = new RAM
        {
            Type = "ddr 5"
        };

        var result = _compatibilityEvaluator.CompareRamMotherboardMemoryType(ram, motherboard);
        Assert.Multiple(() =>
        {
            Assert.That(ram, Is.Not.Null);
            Assert.That(motherboard, Is.Not.Null);
            Assert.That(result, Is.True);
        });
    }

    [Test]
    public void EvalMotherboardGpu_GivesBestScore_WhenPCIeAreSame()
    {
        var motherboard = new Motherboard
        {
            PCIEVersion = 5,
            PcieLanes = 16
        };

        var gpu = new GPU
        {
            PCIeVersion = 5,
            LanesNeeded = 16
        };

        var result = _compatibilityEvaluator.GetGpuMotherboardPcieInterfacesScore(gpu, motherboard);
        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void EvalMotherboardGpu_GivesZero_WhenInputIsNull()
    {
        var motherboard = new Motherboard();
        var gpu = new GPU();

        var result = _compatibilityEvaluator.GetGpuMotherboardPcieInterfacesScore(gpu, motherboard);
        Assert.That(result, Is.EqualTo(0));
    }



    [Test]
    public void EvalMotherboardCpuSocketCompatibility_WhenSocketsAreDifferent_ReturnsFalse()
    {
        var motherboard = new Motherboard
        {
            Socket = "LGA1700"
        };

        var cpu = new CPU
        {
            Socket = "LGA1200"
        };

        var result = _compatibilityEvaluator.CompareCpuMotherboardSockets(cpu, motherboard);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalMotherboardCpuSocketCompatibility_WhenInputIsNull_ReturnsFalse()
    {
        var motherboard = new Motherboard();
        var cpu = new CPU();

        var result = _compatibilityEvaluator.CompareCpuMotherboardSockets(cpu, motherboard);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalMotherboardCaseFormFactor_WhenFactorIsInTheCollection_ReturnsTrue()
    {
        var motherboard = new Motherboard
        {
            FormFactor = "atx"
        };

        var @case = new Case
        {
            SupportedMbFormFactors =
            [
                MbFormFactor.eATX,
                MbFormFactor.ATX,
                MbFormFactor.MicroATX
            ]
        };

        var result = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(@case, motherboard);
        Assert.That(result, Is.True);
    }

    [Test]
    public void EvalMotherboardCaseFormFactor_WhenFactorIsNotInTheCollection_ReturnsFalse()
    {
        var motherboard = new Motherboard
        {
            FormFactor = "Mini DTX"
        };

        var @case = new Case
        {
            SupportedMbFormFactors =
            [
                MbFormFactor.eATX,
                MbFormFactor.ATX,
                MbFormFactor.MicroATX
            ]
        };

        var result = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(@case, motherboard);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalMotherboardCaseFormFactor_WhenInputIsNull_ReturnsFalse()
    {
        var motherboard = new Motherboard
        {
            FormFactor = "Mini dtx"
        };
        var @case = new Case
        {
            SupportedMbFormFactors =
            [
                MbFormFactor.eATX,
                MbFormFactor.ATX,
                MbFormFactor.MicroATX
            ]
        };
        var motherboardNull = new Motherboard();
        var caseNull = new Case();

        var result = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(caseNull, motherboardNull);
        var result2 = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(caseNull, motherboard);
        var result3 = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(@case, motherboardNull);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(result2, Is.False);
            Assert.That(result3, Is.False);
        });
    }
    #endregion
}