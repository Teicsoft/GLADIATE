using System;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public static class CardXmlParser {
    public static Card ParseCardsFromXml(string filePath) {
        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();

        XmlDocument doc = new();
        doc.LoadXml(content);


        XmlNode cardNode = doc.SelectSingleNode("card");

        string cardId = cardNode.Attributes["id"].Value;
        string cardType = cardNode.Attributes["type"].Value;
        string modifier = cardNode.Attributes["modifier"].Value;
        string position = cardNode.Attributes["position"].Value;


        if (!Enum.TryParse(modifier, out Enemy.ModifierEnum parsedModifier)) {
            GD.Print("Failed to parse modifier: " + modifier);
        }

        if (!Enum.TryParse(position, out Enemy.PositionEnum parsedPosition)) {
            GD.Print("Failed to parse position: " + position);
        }


        XmlNode statsNode = cardNode.SelectSingleNode("stats");
        int attack = Utils.ParseIntNode(statsNode, "attack");
        int defenseLower = Utils.ParseIntNode(statsNode, "defense_lower");
        int defenseUpper = Utils.ParseIntNode(statsNode, "defense_upper");
        int health = Utils.ParseIntNode(statsNode, "health");
        int draw = Utils.ParseIntNode(statsNode, "draw");
        int discard = Utils.ParseIntNode(statsNode, "discard");
        int spectaclePoints = Utils.ParseIntNode(statsNode, "spectacle_points");

        XmlNode designNode = cardNode.SelectSingleNode("design");
        string imagePath = Utils.ParseTextNode(designNode, "image");
        string animationPath = Utils.ParseTextNode(designNode, "animation");
        string soundPath = Utils.ParseTextNode(designNode, "sound");

        XmlNode textNode = cardNode.SelectSingleNode("text");
        string name = Utils.ParseTextNode(textNode, "name");
        string description = Utils.ParseTextNode(textNode, "description");
        string lore = Utils.ParseTextNode(textNode, "lore");
        string tooltip = Utils.ParseTextNode(textNode, "tooltip_text");

        return CardFactory.MakeCard(cardType, cardId, parsedModifier, parsedPosition, attack, defenseLower,
            defenseUpper, health, draw, discard, spectaclePoints, name, description, lore, tooltip, imagePath,
            animationPath, soundPath);
    }
}
