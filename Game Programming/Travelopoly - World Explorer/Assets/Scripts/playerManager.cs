using Mono.Data.Sqlite;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    private SqliteConnection connection;
    private string dbPath;
    private string connectionString = "URI=file:gameDatabase.db";

    void Start()
    {
        try
        {
            connection = new SqliteConnection(connectionString);

            if (connection == null)
            {
                Debug.LogError("Database connection could not be created!");
                return;
            }

            connection.Open();
            Debug.Log("Database connection opened successfully!");
        }
        catch (Exception e)
        {
            Debug.Log($"Database connection error: {e.Message}");
        }
    }

    public void UpdatePlayerPosition(int playerId, int newPosition)
    {
        Debug.Log($"in update player position {playerId} {newPosition}");
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE Player SET CurrentPosition = @newPosition WHERE Id = @playerId;";
            command.Parameters.AddWithValue("@newPosition", newPosition);
            command.Parameters.AddWithValue("@playerId", playerId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Debug.Log($"Player {playerId} position updated to {newPosition}.");
            }
            else
            {
                Debug.LogError($"Failed to update position for Player {playerId}");
            }
        }
    }

    public int GetPlayerPosition(int playerId)
    {
        int currentPosition = 0;

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT CurrentPosition FROM Player WHERE Id = @playerId;";
            command.Parameters.AddWithValue("@playerId", playerId);

            object result = command.ExecuteScalar();

            if (result != null)
            {
                currentPosition = Convert.ToInt32(result);
            }
            else
            {
                Debug.LogError($"No position found for Player {playerId} in database!");
            }
        }

        return currentPosition;
    }

    public void CloseConnection()
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
            Debug.Log("SQLite database connection closed.");
        }
    }
}
