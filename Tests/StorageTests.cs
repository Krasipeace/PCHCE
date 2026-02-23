namespace Tests;

using Core;
using Core.Components;
using Core.Enums;

public class StorageTests
{
    private RankEvaluator _rankEvaluator;

    [SetUp]
    public void Setup()
    {
        _rankEvaluator = new RankEvaluator();
    }

    #region RankEvaluator Tests
    [Test]
    public void RankStorage_NVMe_HighSpecs_ReturnsAtLeast80()
    {
        var storage = new Storage
        {
            StorageType = StorageType.Nvme,
            ReadSpeed = 14800,
            WriteSpeed = 13400
        };

        var result = _rankEvaluator.RankStorage(storage);
        Assert.That(result, Is.AtLeast(80));
    }

    [Test]
    public void RankStorage_ZeroSpecs_ReturnsZero()
    {
        var storage = new Storage();
        var result = _rankEvaluator.RankStorage(storage);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void RankStorage_ExceedsMaxSpecs_ClampsTo100()
    {
        var storage = new Storage
        {
            StorageType = StorageType.Nvme,
            ReadSpeed = 16000,
            WriteSpeed = 16000
        };

        var result = _rankEvaluator.RankStorage(storage);
        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void RankStorage_SSD_WorksCorrectly()
    {
        var storage = new Storage
        {
            StorageType = StorageType.Ssd,
            ReadSpeed = 550,
            WriteSpeed = 450
        };

        var result = _rankEvaluator.RankStorage(storage);
        Assert.That(result, Is.AtLeast(5));
        Assert.That(result, Is.LessThanOrEqualTo(50));
    }

    [Test]
    public void RankStorage_Nvme_MediumSpecs_ReturnsAtLeast50()
    {
        var storage = new Storage
        {
            StorageType = StorageType.Nvme,
            ReadSpeed = 7250,
            WriteSpeed = 6300
        };

        var result = _rankEvaluator.RankStorage(storage);
        Assert.That(result, Is.AtLeast(50));
    }

    [Test]
    public void RankStorage_HDD_WorksCorrectly()
    {
        var storage = new Storage
        {
            StorageType = StorageType.Hdd,
            ReadSpeed = 140,
            WriteSpeed = 130
        };

        var result = _rankEvaluator.RankStorage(storage);
        Assert.That(result, Is.AtLeast(1));
        Assert.That(result, Is.AtMost(10));
    }
    #endregion
}
