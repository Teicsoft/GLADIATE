using System.Collections.Generic;
using System.Xml;
using System;
using Godot;
using TeicsoftSpectacleCards.scripts.autoloads;
using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public class EnemyXmlParser
{
    public static Enemy ParseEnemyFromXml(string filePath)
    {
        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        
        XmlDocument doc = new();
        doc.LoadXml(content);
        XmlNode enemyNode = doc.SelectSingleNode("enemy");
        
        string enemyId = enemyNode.Attributes["id"].Value;
        string enemyName = enemyNode.Attributes["name"].Value;
        
        string enemyImage = Utils.ParseTextNode(enemyNode, "image");
        string enemySoundEffect = Utils.ParseTextNode(enemyNode, "sound_effect");
        string enemyLore = Utils.ParseTextNode(enemyNode, "lore");
        string enemyDeckId = Utils.ParseTextNode(enemyNode, "deck_id");
        
        int enemyHealth = Utils.ParseIntNode(enemyNode, "max_health");
        GD.Print("Enemy health: " + enemyHealth.ToString());
        int enemyBlockUpper = Utils.ParseIntNode(enemyNode, "starting_block_upper");
        int enemyBlockLower = Utils.ParseIntNode(enemyNode, "starting_block_lower");
        

        var scene = GD.Load<PackedScene>("res://scenes/battle/Enemy.tscn");
        Enemy enemy = (Enemy) scene.Instantiate();
        
        enemy.InitializeEnemy(enemyId, enemyName, enemyImage, enemySoundEffect, enemyLore, enemyDeckId, enemyHealth, enemyBlockUpper,
        enemyBlockLower);
        
        GD.Print("Finished ParseEnemyFromXml");
        return enemy;
    }
    
    public static List<Enemy> ParseAllEnemies()
    {
        GD.Print("Starting ParseAllEnemies");
        string enemyFilePath = "res://data/enemies/";
        
        string[] filesAtPath = DirAccess.GetFilesAt(enemyFilePath);
        GD.Print("Got files at path");
        
        List<Enemy> enemies = new();
        foreach (string fileName in filesAtPath)
        {
            GD.Print("\nParsing file: " + fileName + " as enemy");
            if (fileName.EndsWith(".xml") && fileName != "enemy_template.xml")
            {
                Enemy enemy = ParseEnemyFromXml(enemyFilePath + fileName);
                GD.Print( "Parsed enemy: " + enemy.ToString() +"\n");
                enemies.Add(enemy);
            }
        }

        GD.Print("Finished ParseAllEnemies");
        return enemies;
    }
}