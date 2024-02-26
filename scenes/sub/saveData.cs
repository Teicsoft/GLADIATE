using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FileAccess = Godot.FileAccess;
namespace GLADIATE.scenes.sub;

public static class SaveData
{
    public static List<ScoreObject> Scores;
    private static string _filePath = "user://save_game.json";
    public static void ParseJson()
    {
        if (FileAccess.FileExists(_filePath))
        {
            using var file = FileAccess.Open(_filePath, FileAccess.ModeFlags.Read);
            var jsonContent = file.GetAsText();
            Scores = JsonSerializer.Deserialize<List<ScoreObject>>(jsonContent);
        } else { Scores = new List<ScoreObject>(); }
    }
    
    public static void WriteScoretoJson(string deckId, int score)
    {
        ParseJson();
        Scores.Add(new ScoreObject
        {
            DeckId = deckId,
            Score = score,
            Date = DateTime.Now
        });

        string jsonContent = JsonSerializer.Serialize(Scores);
        using var file = FileAccess.Open(_filePath, FileAccess.ModeFlags.Write);
        file.StoreString(jsonContent);
    }
    
    public static ScoreObject GetHighScoreByDeck(string deckId)
    {
        ParseJson();
        if (Scores != null)
        {
            ScoreObject score = Scores.FindAll(x => x.DeckId == deckId).MaxBy(x => x.Score);
            if (score != null) { return score; }
        }
        return null;
    }
}

public class ScoreObject
{
    public string DeckId {get ; set;}
    public int Score {get ; set;}
    public DateTime Date {get ; set;}
}



