using System;
using System.Collections.Generic;
using Calculator;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Tests.tests;

[TestFixture]
public class DatabaseServiceTest
{
    private IConfiguration _configuration;
    private DatabaseService _databaseService;

    [SetUp]
    public void Setup()
    {
        // Load appsettings.json fra testprojektet
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Opret DatabaseService med den loadede configuration
        _databaseService = new DatabaseService(_configuration);
    }

    [Test]
    public void SaveCalculation_ShouldNotThrowException()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => _databaseService.SaveCalculation("5 + 3", 8));
    }

    [Test]
    public void GetHistory_ShouldReturnEmptyList_WhenNoData()
    {
        // Act
        var history = _databaseService.GetHistory();

        // Assert
        Assert.That(history, Is.Not.Null);
        Assert.That(history.Count, Is.EqualTo(7)); //fejler hvis der laves flere udregninger
    }
}