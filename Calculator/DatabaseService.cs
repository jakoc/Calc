using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = "Server=localhost;Port=3306;Database=calcdb;User=user;Password=password;";
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

    public List<object> GetHistory()
    {
        var history = new List<object>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT expression, result, created_at FROM calculation_history ORDER BY created_at DESC";
            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    history.Add(new
                    {
                        Expression = reader.GetString(0),
                        Result = reader.GetDouble(1),
                        CreatedAt = reader.GetDateTime(2).ToString("yyyy-MM-dd HH:mm:ss") // Konverterer dato til string
                    });
                }
            }
        }
        return history;
    }
}