using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Calculator;
public class DatabaseService 
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Database");

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Database connection string is not set. Please configure it in appsettings.json or environment variables.");
        }
    }

    public void SaveCalculation(string expression, double result)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO calculation_history (expression, result) VALUES (@expression, @result)";
            using (var cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@expression", expression);
                cmd.Parameters.AddWithValue("@result", result);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<(string Expression, double Result, DateTime CreatedAt)> GetHistory()
    {
        var history = new List<(string, double, DateTime)>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT expression, result, created_at FROM calculation_history ORDER BY created_at DESC";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    history.Add((reader.GetString(0), reader.GetDouble(1), reader.GetDateTime(2)));
                }
            }
        }
        return history;
    }
    public class CalculationHistory
    {
        public string Expression { get; set; }
        public double Result { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}