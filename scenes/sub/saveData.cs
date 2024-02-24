using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GLADIATE.scripts.XmlParsing;
using Godot;
using FileAccess = Godot.FileAccess;
namespace GLADIATE.scenes.sub;

public static class saveData
{
    private static List<ScoreObject> scores;
    private static string filePath = "user://save_game.json";
    public static void ParseJson()
    {
        if (File.Exists(filePath))
        {
            using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
            var jsonContent = File.ReadAllText(filePath);
            scores = JsonSerializer.Deserialize<List<ScoreObject>>(jsonContent);
        }
        else { scores = new List<ScoreObject>(); }
    }
    
    public static void WriteScoretoJSON(string deckId, int score)
    {
        ParseJson();
        scores.Add(new ScoreObject
        {
            DeckId = deckId,
            Score = score,
            Date = DateTime.Now
        });

        string jsonContent = JsonSerializer.Serialize(scores);
        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
        file.StoreString(jsonContent);
    }
}

public class ScoreObject
{
    public string DeckId {get ; set;}
    public int Score {get ; set;}
    public DateTime Date {get ; set;}
}

