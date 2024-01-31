using System.Collections.Generic;
using System.Xml;
using System;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public class BattleXmlParser
{
    public static Dictionary<string, dynamic> ParseBattleFromXml(string filePath)
    {
        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        
        XmlDocument doc = new();
        doc.LoadXml(content);
        XmlNode battleNode = doc.SelectSingleNode("battle");
        
        string battleId = battleNode.Attributes["id"].Value;
        string battleName = battleNode.Attributes["name"].Value;
        string music = Utils.ParseTextNode(battleNode, "music");

        List<string> enemies = new();
        foreach (XmlNode enemyNode in battleNode.SelectNodes("enemies"))
        {
            string enemyId = enemyNode.Attributes["enemy_id"].Value;
            enemies.Add(enemyId);
        }
        
        Dictionary<string, dynamic> battleData = new();
            battleData.Add("battle_id", battleId);
            battleData.Add("battle_name", battleName);
            battleData.Add("music", music);
            battleData.Add("enemies", enemies);

            return battleData;
    }
    
    public static List<Dictionary<string, dynamic>> ParseAllBattles()
    {
        string battleFilePath = "res://data/enemies/";
        
        string[] filesAtPath = DirAccess.GetFilesAt(battleFilePath);
        
        List<Dictionary<string, dynamic>> battles = new();
        foreach (string fileName in filesAtPath)
        {
            if (fileName.EndsWith(".xml") && fileName != "battle_template.xml")
            {
                Dictionary<string, dynamic> battleData = ParseBattleFromXml(battleFilePath + fileName);
                battles.Add(battleData);
            }
        }

        return battles;
    }
}