using System.Collections.Generic;
using System.Xml;
using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.XmlParsing;

public class EnemyXmlParser {
    public static Enemy ParseEnemyFromXml(string filePath) {
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
        int enemyBlockUpper = Utils.ParseIntNode(enemyNode, "starting_block_upper");
        int enemyBlockLower = Utils.ParseIntNode(enemyNode, "starting_block_lower");

        Enemy enemy = new();

        enemy.InitializeEnemy(
            enemyId, enemyName, enemyImage, enemySoundEffect, enemyLore, enemyDeckId, enemyHealth, enemyBlockUpper,
            enemyBlockLower
        );

        return enemy;
    }

    public static List<Enemy> ParseAllEnemies() {
        string enemyFilePath = "res://data/enemies/";

        string[] filesAtPath = DirAccess.GetFilesAt(enemyFilePath);

        List<Enemy> enemies = new();
        foreach (string fileName in filesAtPath) {
            if (fileName.EndsWith(".xml") && fileName != "enemy_template.xml") {
                Enemy enemy = ParseEnemyFromXml(enemyFilePath + fileName);
                enemies.Add(enemy);
            }
        }

        return enemies;
    }
}
