using System;
using System.Collections.Generic;
using Calculator;
using Microsoft.Extensions.Configuration;
using Moq;
using MySql.Data.MySqlClient;
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
        Assert.That(history.Count, Is.EqualTo(0)); //fejler hvis der laves flere udregninger
    }
    
    [Test]
    public void SaveCalculation_ShouldInsertDataIntoDatabase()
    {
        // Arrange
        var expression = "5 + 3";
        var result = 8.0;

        // Act
        Assert.DoesNotThrow(() => _databaseService.SaveCalculation(expression, result));
        double tolerance = 0.0001;
        // Assert - Tjek at den er gemt ved at hente historikken
        var history = _databaseService.GetHistory();
        Assert.That(history, Is.Not.Null);
        Assert.That(history.Count, Is.GreaterThan(0));
        Assert.That(history.Any(h => h.Expression == expression && Math.Abs(h.Result - result) < tolerance), 
            Is.True, "Den indsatte beregning findes ikke i historikken.");
    }
    
    [Test]
    public void GetHistory_ShouldReturnEmptyList_WhenNoDataExists()
    {
        // Arrange - Ryd databasen før test
        using var connection = new MySqlConnection(_configuration.GetConnectionString("Database"));
        connection.Open();
        new MySqlCommand("DELETE FROM calculation_history", connection).ExecuteNonQuery();

        // Act
        var history = _databaseService.GetHistory();

        // Assert
        Assert.That(history, Is.Not.Null);
        Assert.That(history.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void GetHistory_ShouldReturnMultipleEntries()
    {
        // Arrange
        _databaseService.SaveCalculation("2 + 2", 4);
        _databaseService.SaveCalculation("3 * 3", 9);

        // Act
        var history = _databaseService.GetHistory();

        // Assert
        Assert.That(history, Is.Not.Null);
        Assert.That(history.Count, Is.GreaterThanOrEqualTo(2));
    }
    
    
    [Test]
    public void ClearHistory_ShouldRemoveAllEntries()
    {
        // Arrange - Tilføj nogle testberegninger
        _databaseService.SaveCalculation("2 + 2", 4);
        _databaseService.SaveCalculation("3 * 3", 9);

        // Act - Slet alle beregninger
        using var connection = new MySqlConnection(_configuration.GetConnectionString("Database"));
        connection.Open();
        new MySqlCommand("DELETE FROM calculation_history", connection).ExecuteNonQuery();

        // Assert - Tjek at databasen nu er tom
        var history = _databaseService.GetHistory();
        Assert.That(history, Is.Not.Null);
        Assert.That(history.Count, Is.EqualTo(0));
    }
    
}