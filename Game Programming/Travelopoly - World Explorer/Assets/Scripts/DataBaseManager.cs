using System;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using TMPro;
using System.Collections.Generic;



public class DatabaseManager : MonoBehaviour
{
    private string dbPath;
    private SqliteConnection connection;
    private PlayerManager playerManager;
    public PropertyUIManager propertyUIManager;
    private string connectionString = "URI=file:gameDatabase.db"; 

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        propertyUIManager = FindObjectOfType<PropertyUIManager>();
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

            CreateTables();
            InsertInitialValuesIfNeeded();
            ClearPrevGameData();
        }
        catch (Exception e)
        {
            Debug.LogError($"Database connection error: {e.Message}");
        }
    }

    void CreateTables()
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Property (
                    Id INTEGER PRIMARY KEY,
                    Position INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    Cost REAL NOT NULL,
                    BaseRent REAL NOT NULL,
                    Rent1 REAL NOT NULL,
                    Rent2 REAL NOT NULL,
                    Mortgage REAL NOT NULL,
                    ColorCategory TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Player (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Balance REAL NOT NULL,
                    CurrentPosition INTEGER NOT NULL,
                    InJail BOOLEAN NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Transactions (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    property_id INTEGER NOT NULL,
                    owner_id INTEGER NOT NULL,
                    transaction_type TEXT NOT NULL, 
                    rent_type TEXT DEFAULT NULL, 
                    is_mortgaged BOOLEAN DEFAULT 0, 
                    attraction_level INTEGER DEFAULT 0,
                    FOREIGN KEY(property_id) REFERENCES Property(Id),
                    FOREIGN KEY(owner_id) REFERENCES Player(Id)
                );

               

                CREATE TABLE IF NOT EXISTS PlayerPerformance (
                PlayerId INTEGER,
                QuestionId INTEGER,
                Correct INTEGER DEFAULT 0,
                Attempts INTEGER DEFAULT 0,
                PRIMARY KEY (PlayerId, QuestionId)
                );

                CREATE TABLE IF NOT EXISTS OwnedProperties (
                PlayerId INTEGER,
                Position INTEGER
                );
                
              

    
            ";
            command.ExecuteNonQuery();
        }
    }

    void InsertInitialValuesIfNeeded()
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT COUNT(*) FROM Property;";
            int count = Convert.ToInt32(command.ExecuteScalar());

            if (count == 0)
            {
                Debug.Log("Database is empty, inserting initial values...");
                InsertInitialValues();
            }
            else
            {
                Debug.Log("Database already has data, skipping insertion.");
            }
        }
    }

    void InsertInitialValues()
    {
        using (var command = connection.CreateCommand())
        {
            try
            {
                command.CommandText = "BEGIN TRANSACTION;";
                command.ExecuteNonQuery();

                string[] insertQueries = new string[]
                {
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (1, 1, 'New Delhi', 350, 35, 70, 120, 240,'Lavender');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (2, 3, 'Turkey Istanbul Airport', 1000, 200, 350, 500, 850,'White');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (3, 4, 'Munich', 500, 50, 100, 150, 360,'Lavender');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (4, 5, 'New York', 1300, 130, 270, 400, 1180,'Green');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (5, 7, 'Mumbai', 400, 40, 80, 120, 250,'Yellow');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (6, 9, 'Hongkong International Airport', 1200, 250, 400, 550, 950,'White');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (7, 10, 'Singapore', 800, 100, 200, 320, 650,'Yellow');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (8, 11, 'Seoul', 750, 70, 150, 220, 580,'Green');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (9, 13, 'Paris', 850, 80, 170, 250, 680,'Green');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (10, 14, 'Sydney', 600, 60, 120, 150, 450,'Yellow');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (11, 15, 'Frankfurt International Airport', 1100, 220, 360, 480, 920,'White');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (12, 17, 'Dubai', 1200, 120, 250, 370, 1080,'Peach');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (13, 19, 'London', 1000, 100, 200, 320, 780,'Lavender');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (14, 20, 'Tokyo', 900, 90, 190, 280, 780,'Peach');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (15, 21, 'Newark International Airport', 1300, 300, 570, 780, 1000,'White');",
                    "INSERT INTO Property (Id, Position, Name, Cost, BaseRent, Rent1, Rent2, Mortgage, ColorCategory) VALUES (16, 23, 'Los Angeles', 650, 65, 140, 220, 530,'Peach');",

                    "INSERT INTO Player (Id, Name, Balance, CurrentPosition, InJail) VALUES (1, 'Player 1', 5000, 0, 0);",
                    "INSERT INTO Player (Id, Name, Balance, CurrentPosition, InJail) VALUES (2, 'Player 2', 5000, 0, 0);"
                };

                foreach (string query in insertQueries)
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }

                command.CommandText = "COMMIT;";
                command.ExecuteNonQuery();

                Debug.Log("Data inserted successfully.");
            }
            catch (Exception e)
            {
                command.CommandText = "ROLLBACK;";
                command.ExecuteNonQuery();
                Debug.LogError("SQL Insert Error: " + e.Message);
            }
        }
    }

    void ClearPrevGameData()
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            DELETE FROM Transactions;
            DELETE FROM OwnedProperties;
            DELETE FROM PlayerPerformance;
        ";
            command.ExecuteNonQuery();
            Debug.Log("Cleared Transactions, OwnedProperties, and PlayerPerformance tables.");
        }
    }


    public float GetPlayerBalance(int playerId)
    {
        
        if (connection == null)
        {
            Debug.LogError("Database connection is NULL in GetPlayerBalance!");
            return 0f;
        }
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Balance FROM Player WHERE Id = @playerId;";
            command.Parameters.AddWithValue("@playerId", playerId);

            object result = command.ExecuteScalar();

            if (result == null)
            {
                Debug.LogError($"No balance found for Player {playerId} in database!");
                return 0f;
            }

            if (float.TryParse(result.ToString(), out float balance))
            {
                Debug.Log($"Player {playerId} balance fetched: Rs. {balance}");
                return balance;
            }
            else
            {
                Debug.LogError($"Failed to parse balance for Player {playerId}");
                return 0f;
            }
        }
    }


    public void UpdatePlayerNames(string player1Name, string player2Name)
    {
        
        
        using (var command = connection.CreateCommand())
        {
            
            command.CommandText = "UPDATE Player SET Name = @player1Name, Balance = 5000, CurrentPosition = 0 WHERE Id = 1;";

            command.Parameters.AddWithValue("@player1Name", player1Name);
            command.ExecuteNonQuery();
        }

        
        using (var command = connection.CreateCommand())
        {
            
            command.CommandText = "UPDATE Player SET Name = @player2Name, Balance = 5000, CurrentPosition = 0 WHERE Id = 2;";

            command.Parameters.AddWithValue("@player2Name", player2Name);
            command.ExecuteNonQuery();
        }

        Debug.Log("Player names updated in the database.");
    }
    public void UpdatePlayerBalance(int playerId, float newBalance)
    {
        Debug.Log($"Updating Player {playerId} Balance...");
        Debug.Log($"New Balance to Set: Rs.{newBalance}");

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE Player SET Balance = @newBalance WHERE Id = @playerId;";
            command.Parameters.AddWithValue("@newBalance", newBalance);
            command.Parameters.AddWithValue("@playerId", playerId);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                Debug.Log($"Player {playerId} Balance Successfully Updated: Rs.{newBalance}");
            }
            else
            {
                Debug.LogError($"ERROR: Balance NOT Updated for Player {playerId}!");
            }
        }
    }

    public PropertyData GetPropertyDetailsByPosition(int position)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Id, Name, Cost, ColorCategory, Position FROM Property WHERE Position = @position;";
            command.Parameters.AddWithValue("@position", position);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new PropertyData(
                        reader.GetInt32(0),  // Id
                        reader.GetString(1), // Name
                        reader.GetFloat(2),  // Cost
                        reader.GetString(3), // ColorCategory
                        reader.GetInt32(4)   // Position
                    );
                }
            }
        }
        return null;
    }



    public int GetPropertyOwner(int propertyId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            SELECT owner_id FROM Transactions
            WHERE property_id = @propertyId AND transaction_type = 'purchase'
            ORDER BY id DESC LIMIT 1;
        ";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0; 
        }
    }

    public int GetPropertyAttractionLevel(int propertyId, int ownerId)
    {
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            SELECT attraction_level FROM Transactions
            WHERE property_id = @propertyId AND transaction_type = 'upgrade' AND owner_id = @ownerId
            ORDER BY id DESC LIMIT 1;
        ";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            command.Parameters.AddWithValue("@ownerId", ownerId);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0; 
        }
    }

    public int GetPropertyId(int position)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            SELECT id FROM Property
            WHERE position = @position
            ORDER BY id DESC LIMIT 1;
        ";
            command.Parameters.AddWithValue("@position", position);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0; 
        }
    }

    public float GetPropertyCost(int propertyId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Cost FROM Property WHERE Id = @propertyId;";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToSingle(result) : 0f;
        }
    }

    public void LogPropertyPurchase(int propertyId, int ownerId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            INSERT INTO Transactions (property_id, owner_id, transaction_type, attraction_level)
            VALUES (@propertyId, @ownerId, 'purchase', 0);
        ";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            command.Parameters.AddWithValue("@ownerId", ownerId);
            command.ExecuteNonQuery();
        }
        var position = GetPropertyPositionById(propertyId);
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            INSERT INTO OwnedProperties (PlayerId, Position)
            VALUES (@ownerId, @position);
        ";
            command.Parameters.AddWithValue("@ownerId", ownerId);
            command.Parameters.AddWithValue("@position", position);
            command.ExecuteNonQuery();
        }
    }

    public int GetCurrentAttractionLevel(int propertyId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            SELECT attraction_level FROM Transactions
            WHERE property_id = @propertyId AND transaction_type = 'upgrade'
            ORDER BY id DESC LIMIT 1;
        ";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0; 
        }
    }


    public float GetAttractionCost(int propertyId)
    {
        int attractionLevel = GetCurrentAttractionLevel(propertyId);

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            SELECT 
                CASE 
                    WHEN @attractionLevel = 0 THEN BaseRent
                    WHEN @attractionLevel = 1 THEN Rent1
                    WHEN @attractionLevel = 2 THEN Rent2
                    ELSE BaseRent
                END AS AttractionCost
            FROM Property WHERE Id = @propertyId;
        ";

            command.Parameters.AddWithValue("@propertyId", propertyId);
            command.Parameters.AddWithValue("@attractionLevel", attractionLevel);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToSingle(result) : 0f;
        }
    }

    public void LogAttractionUpgrade(int propertyId, int ownerId)
    {
        int currentLevel = GetCurrentAttractionLevel(propertyId);
        int newLevel = Mathf.Min(currentLevel + 1, 2); // Max attraction level is 2

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            INSERT INTO Transactions (property_id, owner_id, transaction_type, attraction_level)
            VALUES (@propertyId, @ownerId, 'upgrade', @newLevel);
        ";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            command.Parameters.AddWithValue("@ownerId", ownerId);
            command.Parameters.AddWithValue("@newLevel", newLevel);
            command.ExecuteNonQuery();
        }
    }

    public float GetUpdatedRent(int propertyId)
    {
        int attractionLevel = GetCurrentAttractionLevel(propertyId);

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            SELECT 
                CASE 
                    WHEN @attractionLevel = 0 THEN BaseRent
                    WHEN @attractionLevel = 1 THEN Rent1
                    WHEN @attractionLevel = 2 THEN Rent2
                    ELSE BaseRent
                END AS RentAmount
            FROM Property WHERE Id = @propertyId;
        ";

            command.Parameters.AddWithValue("@propertyId", propertyId);
            command.Parameters.AddWithValue("@attractionLevel", attractionLevel);
            object result = command.ExecuteScalar();

            return result != null ? Convert.ToSingle(result) : 0f;
        }
    }

    public void LogRentPayment(int propertyId, int ownerId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            INSERT INTO Transactions (property_id, owner_id, transaction_type)
            VALUES (@propertyId, @ownerId, 'rent');
        ";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            command.Parameters.AddWithValue("@ownerId", ownerId);
            command.ExecuteNonQuery();
        }
    }

    public void LogMortgage(int propertyId, int ownerId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            INSERT INTO Transactions (property_id, owner_id, transaction_type)
            VALUES (@propertyId, @ownerId, 'mortgage');
        ";
            command.Parameters.AddWithValue("@propertyId", propertyId);
            command.Parameters.AddWithValue("@ownerId", ownerId);
            command.ExecuteNonQuery();
        }
    }

    public List<int> GetPlayerOwnedProperties(int playerId)
    {
        List<int> ownedPositions = new List<int>();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Position FROM OwnedProperties WHERE PlayerId = @playerId;";
            command.Parameters.AddWithValue("@playerId", playerId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ownedPositions.Add(reader.GetInt32(0));
                }
            }
        }
        
        return ownedPositions;
    }


    public void UpdatePlayerPerformance(int playerId, int questionId, bool isCorrect)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
            INSERT INTO PlayerPerformance (PlayerId, QuestionId, Correct, Attempts)
            VALUES (@playerId, @questionId, @correct, 1)
            ON CONFLICT(PlayerId, QuestionId) 
            DO UPDATE SET 
                Correct = Correct + @correct, 
                Attempts = Attempts + 1;
        ";
            command.Parameters.AddWithValue("@playerId", playerId);
            command.Parameters.AddWithValue("@questionId", questionId);
            command.Parameters.AddWithValue("@correct", isCorrect ? 1 : 0);
            command.ExecuteNonQuery();
        }
    }

    public Dictionary<int, float> GetPlayerTriviaAccuracy(int playerId)
    {
        Dictionary<int, float> accuracyData = new Dictionary<int, float>();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT QuestionId, Correct, Attempts FROM PlayerPerformance WHERE PlayerId = @playerId;";
            command.Parameters.AddWithValue("@playerId", playerId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int questionId = reader.GetInt32(0);
                    int correct = reader.GetInt32(1);
                    int attempts = reader.GetInt32(2);
                    accuracyData[questionId] = (float)correct / attempts;
                }
            }
        }
        return accuracyData;
    }

    public int GetTotalAttempts(int playerId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT SUM(Attempts) FROM PlayerPerformance WHERE PlayerId = @playerId;";
            command.Parameters.AddWithValue("@playerId", playerId);

            object result = command.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }

    public void AddOwnedProperty(int playerId, int position)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO OwnedProperties (PlayerId, Position) VALUES (@playerId, @position);";
            command.Parameters.AddWithValue("@playerId", playerId);
            command.Parameters.AddWithValue("@position", position);
            command.ExecuteNonQuery();
        }
    }

    public int GetPropertyPositionById(int id)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Position FROM Property WHERE Id = @id;";
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return reader.GetInt32(0);
            }
        }
        Debug.LogWarning($"Position not found for Property Id {id}");
        return -1;
    }

    public int GetPlayerPosition(int playerId)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT CurrentPosition FROM Player WHERE Id = @playerId;";
            command.Parameters.AddWithValue("@playerId", playerId);

            object result = command.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out int position))
            {
                return position;
            }
            else
            {
                Debug.LogError($"Failed to get position for Player {playerId}");
                return 0;
            }
        }
    }

    public void RecordTriviaAttempt(int playerId, int questionId, bool isCorrect)
    {
        using (var command = connection.CreateCommand())
        {
           
            command.CommandText = "SELECT Correct, Attempts FROM PlayerPerformance WHERE PlayerId = @playerId AND QuestionId = @questionId;";
            command.Parameters.AddWithValue("@playerId", playerId);
            command.Parameters.AddWithValue("@questionId", questionId);

            int correct = 0, attempts = 0;
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    correct = reader.GetInt32(0);
                    attempts = reader.GetInt32(1);
                }
            }

            if (isCorrect == true)
            {
                correct = correct + 1;
            }
            if (isCorrect == false)
            {
                attempts = attempts + 1;
            }

            Debug.Log($"The value of correct is {correct}");
            Debug.Log($"The value of attempts is {attempts}");
            
            command.CommandText = @"
SELECT COUNT(*) 
FROM PlayerPerformance 
WHERE PlayerId = @playerId AND QuestionId = @questionId";

            
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@playerId", playerId);  
            command.Parameters.AddWithValue("@questionId", questionId);  

            // Execute the check query
            int recordCount = Convert.ToInt32(command.ExecuteScalar());

            // Prepare the update or insert query based on record existence
            if (recordCount > 0)
            {
                command.CommandText = @"
    UPDATE PlayerPerformance
    SET Correct = @correct, Attempts = @attempts
    WHERE PlayerId = @playerId AND QuestionId = @questionId";
            }
            else
            {
                command.CommandText = @"
    INSERT INTO PlayerPerformance (PlayerId, QuestionId, Correct, Attempts)
    VALUES (@playerId, @questionId, @correct, @attempts)";
            }

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@playerId", playerId);  
            command.Parameters.AddWithValue("@questionId", questionId);  
            command.Parameters.AddWithValue("@correct", correct);  
            command.Parameters.AddWithValue("@attempts", attempts);  

           
            command.ExecuteNonQuery();
           
        }
    }


    public int GetAttemptCount(int playerId, int questionId)
    {

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Attempts FROM PlayerPerformance WHERE PlayerId = @playerId AND QuestionId = @questionId;";
            command.Parameters.AddWithValue("@playerId", playerId);
            command.Parameters.AddWithValue("@questionId", questionId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return reader.GetInt32(0);
            }
        }
        return 0;
    }


    public void UpdatePlayerAttempts(int playerId, int questionId, int newAttempts)
    {
        
        using (var command = connection.CreateCommand())
        {

            command.CommandText = @"
            UPDATE PlayerPerformance
            SET Attempts = @newAttempts
            WHERE PlayerId = @playerId AND QuestionId = @questionId";

           
            command.Parameters.AddWithValue("@newAttempts", newAttempts);
            command.Parameters.AddWithValue("@playerId", playerId);
            command.Parameters.AddWithValue("@questionId", questionId);

            
            command.ExecuteNonQuery();

        }
    }

    

    public void SaveGame(int playerId, int newPosition, float newBalance)
    {
        
        if (connection == null)
        {
            Debug.LogError("ERROR: Database connection is NULL! Ensure it is initialized.");
            return;
        }

       

        try
        {
            // Update player position
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Player SET CurrentPosition = @newPosition WHERE Id = @playerId;";
                command.Parameters.AddWithValue("@newPosition", newPosition);
                command.Parameters.AddWithValue("@playerId", playerId);
                command.ExecuteNonQuery();
            }

            // Update player balance
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Player SET Balance = @newBalance WHERE Id = @playerId;";
                command.Parameters.AddWithValue("@newBalance", newBalance);
                command.Parameters.AddWithValue("@playerId", playerId);
                command.ExecuteNonQuery();
            }

            Debug.Log($"Game progress saved: Player {playerId} → Position {newPosition}, Balance {newBalance}");
        }
        catch (Exception e)
        {
            Debug.LogError($"ERROR saving game progress: {e.Message}");
        }
    }


    void OnDestroy()
    {
        if (connection != null)
        {
            connection.Close();
            Debug.Log("Database connection closed.");
        }
        else
        {
            Debug.LogWarning("Database connection was already null in OnDestroy.");
        }
    }
}




