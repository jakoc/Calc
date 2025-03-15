using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Calculator.Controllers;
using Calculator;
using System;

namespace Tests.tests;

[TestFixture]
public class CalculatorControllerTest
{
    private Mock<IDatabaseService> _mockDbService;
    private CalculatorController _controller;

    [SetUp]
    public void Setup()
    {
        _mockDbService = new Mock<IDatabaseService>();
        _controller = new CalculatorController(_mockDbService.Object);
    }

    [Test]
    public void GetHistory_ShouldReturnNotFound_WhenNoHistoryExists()
    {
        // Arrange
        _mockDbService.Setup(db => db.GetHistory()).Returns(new List<(string, double, DateTime)>());

        // Act
        var result = _controller.GetHistory() as NotFoundObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(404));
    }

    [Test]
    public void GetHistory_ShouldReturnOk_WhenHistoryExists()
    {
        // Arrange
        var history = new List<(string, double, DateTime)>
        {
            ("5 + 3", 8, DateTime.UtcNow)
        };
        _mockDbService.Setup(db => db.GetHistory()).Returns(history);

        // Act
        var result = _controller.GetHistory() as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
}

