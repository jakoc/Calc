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
    public void Calculate_ShouldReturnBadRequest_WhenRequestIsNull()
    {
        var result = _controller.Calculate(null!) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));
        
    }

    [Test]
    public void Calculate_ShouldReturnBadRequest_WhenExpressionIsMissing()
    {
        var request = new CalculationRequest { Expression = "", CalculatorType = "simple" };
        var result = _controller.Calculate(request) as BadRequestObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));
        
    }
    
    
    [Test]
    public void Calculate_ShouldReturnBadRequest_WhenCalculationThrowsException()
    {
        var request = new CalculationRequest { Expression = "5 / 0", CalculatorType = "simple" };

        var result = _controller.Calculate(request) as BadRequestObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));
        
    }
    
    [Test]
    public void Calculate_ShouldReturnInternalServerError_WhenDatabaseSaveFails()
    {
        var request = new CalculationRequest { Expression = "5 + 3", CalculatorType = "simple" };

        _mockDbService.Setup(db => db.SaveCalculation(It.IsAny<string>(), It.IsAny<double>()))
            .Throws(new Exception("Database fejl"));

        var result = _controller.Calculate(request) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(500));
        
    }
    
    [Test]
    public void Calculate_ShouldReturnOk_WhenValidRequest()
    {
        var request = new CalculationRequest { Expression = "5 + 3", CalculatorType = "simple" };
        var result = _controller.Calculate(request) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
    
    [Test]
    public void GetHistory_ShouldReturnNotFound_WhenNoHistoryExists()
    {
        _mockDbService.Setup(db => db.GetHistory()).Returns(new List<(string, double, DateTime)>());
        var result = _controller.GetHistory() as NotFoundObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(404));
    }
    
    [Test]
    public void GetHistory_ShouldReturnOk_WhenHistoryExists()
    {
        var history = new List<(string, double, DateTime)>
        {
            ("5 + 3", 8, DateTime.UtcNow)
        };
        _mockDbService.Setup(db => db.GetHistory()).Returns(history);
        var result = _controller.GetHistory() as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }
    
    
}

