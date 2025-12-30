namespace Tests;

using Core;
using Core.Components;
using Core.Enums;

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
            PCIEVersion = PCIeVersion.PCIe_5_0,
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
            PCIEVersion = PCIeVersion.PCIe_6_0,
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
            MemoryType = MemoryType.DDR_5
        };

        var ram = new RAM
        {
            Type = MemoryType.DDR_5
        };

        var result = _compatibilityEvaluator.CompareRamMotherboardMemoryType(ram, motherboard);
        Assert.That(result, Is.True);
    }

    [Test]
    public void EvalMotherBoardRam_GivesFalseResult_WhenTypesAreNotTheSame()
    {
        var motherboard = new Motherboard
        {
            MemoryType = MemoryType.DDR_5
        };

        var ram = new RAM
        {
            Type = MemoryType.DDR_4
        };

        var result = _compatibilityEvaluator.CompareRamMotherboardMemoryType(ram, motherboard);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalMotherBoardRam_GivesTrueResult_WhenNormalizeIsInAction()
    {
        var motherboard = new Motherboard
        {
            MemoryType = MemoryType.DDR_5
        };

        var ram = new RAM
        {
            Type = MemoryType.DDR_5
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
            PCIEVersion = PCIeVersion.PCIe_5_0,
            PcieLanes = 16
        };

        var gpu = new GPU
        {
            PCIeVersion = PCIeVersion.PCIe_5_0,
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
            FormFactor = MbFormFactor.ATX
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
            FormFactor = MbFormFactor.MiniITX
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
            FormFactor = MbFormFactor.MiniDTX
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

    [Test]
    public void EvalMotherboardCaseFormFactor_ReturnsTrue_WhenFormFactorMatchesIgnoringCaseSpacesAndDashes()
    {
        var supportedFormFactors = new[]
        {
            "ATX",
            "Micro-ATX",
            " Mini ITX "
        };

        var motherboardFormFactor = "mini-itx";

        var result = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(supportedFormFactors, motherboardFormFactor);
        Assert.That(result, Is.True);
    }

    [Test]
    public void EvalMotherboardCaseFormFactor_ReturnsFalse_WhenFormFactorIsNotSupported()
    {
        var supportedFormFactors = new[]
        {
            "ATX",
            "Micro-ATX"
        };

        var motherboardFormFactor = "E-ATX";

        var result = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(supportedFormFactors, motherboardFormFactor);
        Assert.That(result, Is.False);
    }

    [Test]
    public void EvalMotherboardCaseFormFactor_ReturnsFalse_WhenInputsAreInvalid()
    {
        var emptyFormFactors = Array.Empty<string>();
        string? motherboardFormFactor = null;

        var result = _compatibilityEvaluator.CompareCaseMotherBoardFormFactor(emptyFormFactors, motherboardFormFactor!);
        Assert.That(result, Is.False);
    }
    #endregion
}