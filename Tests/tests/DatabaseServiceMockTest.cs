using Calculator.Controllers;
using Moq;

namespace Tests.tests;

[TestFixture]
public class DatabaseServiceMockTest
{
    private Mock<IDatabaseService> _mockDbService;
    private IDatabaseService _databaseService;

    [SetUp]
    public void Setup()
    {
        _mockDbService = new Mock<IDatabaseService>();
        _databaseService = _mockDbService.Object;
    }

    [Test]
    public void SaveCalculation_ShouldCallDatabaseButNotModifyIt()
    {
        // Arrange
        var expression = "5 + 3";
        var result = 8.0;

        // Act
        _databaseService.SaveCalculation(expression, result);

        // Assert
        _mockDbService.Verify(db => db.SaveCalculation(expression, result), Times.Once);
    }

    [Test]
    public void GetHistory_ShouldReturnMockedData()
    {
        // Arrange
        var expectedHistory = new List<(string, double, DateTime)>
        {
            ("2 + 2", 4, DateTime.UtcNow),
            ("3 * 3", 9, DateTime.UtcNow)
        };
        _mockDbService.Setup(db => db.GetHistory()).Returns(expectedHistory);

        // Act
        var history = _databaseService.GetHistory();

        // Assert
        Assert.That(history, Is.Not.Null);
        Assert.That(history.Count, Is.EqualTo(2));
    }
}