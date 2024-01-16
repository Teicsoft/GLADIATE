using System;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.customresource.Cards;

namespace TeicsoftSpectacleCards.scripts.customresource;

public static class CardXmlParser
{
	public static CardModel ParseCardsFromXml(string filePath)
	{
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(content);
		
		
		XmlNode cardNode = doc.SelectSingleNode("card");
		
		string cardId = cardNode.Attributes["id"].Value;
		string cardType = cardNode.Attributes["type"].Value;
		string modifier = cardNode.Attributes["modifier"].Value;
		string position = cardNode.Attributes["position"].Value;
		
		
		if (Enum.TryParse(modifier, out CardModel.ModifierEnum parsedModifier))
		{}
		else
		{
			GD.Print("Failed to parse modifier: " + modifier);
		}

		if (Enum.TryParse(position, out CardModel.PositionEnum parsedPosition))
		{}
		else
		{
			GD.Print("Failed to parse position: " + position);
		}
		
		
		XmlNode statsNode = cardNode.SelectSingleNode("stats");
		int attack = statsNode.SelectSingleNode("attack") != null ? Int32.Parse(statsNode.SelectSingleNode("attack").InnerText) : 0;
		int defenseLower = statsNode.SelectSingleNode("defense_lower") != null ? Int32.Parse(statsNode.SelectSingleNode("defense_lower").InnerText) : 0;
		int defenseUpper = statsNode.SelectSingleNode("defense_upper") != null ? Int32.Parse(statsNode.SelectSingleNode("defense_upper").InnerText) : 0;
		int health = statsNode.SelectSingleNode("health") != null ? Int32.Parse(statsNode.SelectSingleNode("health").InnerText) : 0;
		int draw = statsNode.SelectSingleNode("draw") != null ? Int32.Parse(statsNode.SelectSingleNode("draw").InnerText) : 0;
		int discard = statsNode.SelectSingleNode("discard") != null ? Int32.Parse(statsNode.SelectSingleNode("discard").InnerText) :0;
		int spectaclePoints = statsNode.SelectSingleNode("spectacle_points") != null ? Int32.Parse(statsNode.SelectSingleNode("spectacle_points").InnerText) : 0;
		
		XmlNode designNode = cardNode.SelectSingleNode("design");
		string imagePath = designNode.SelectSingleNode("image") != null ? designNode.SelectSingleNode("image").InnerText : "";
		string animationPath = designNode.SelectSingleNode("animation") != null ? designNode.SelectSingleNode("animation").InnerText : "";
		string soundPath = designNode.SelectSingleNode("sound") != null ? designNode.SelectSingleNode("sound").InnerText : "";
		
		XmlNode textNode = cardNode.SelectSingleNode("text");
		string name = textNode.SelectSingleNode("name") != null ? textNode.SelectSingleNode("name").InnerText : "";
		string description = textNode.SelectSingleNode("description") != null ? textNode.SelectSingleNode("description").InnerText : "";
		string lore = textNode.SelectSingleNode("lore") != null ? textNode.SelectSingleNode("lore").InnerText : "";
		string tooltip = textNode.SelectSingleNode("tooltip_text") != null ? textNode.SelectSingleNode("tooltip_text").InnerText : "";
		
		CardModel card = CardFactory.MakeCard(
			
			cardType, cardId, parsedModifier, parsedPosition,
			attack, defenseLower, defenseUpper, health, draw, spectaclePoints,
			name, description, lore, tooltip,
			imagePath, animationPath, soundPath
		);

        return card;

        int ParseIntNode(XmlNode parent, string s) {
            return parent.SelectSingleNode(s) != null ? Int32.Parse(parent.SelectSingleNode(s).InnerText) : 0;
        }

        string ParseTextNode(XmlNode parent, string s) {
            return parent.SelectSingleNode(s) != null ? parent.SelectSingleNode(s).InnerText : "";
        }
    }
}
