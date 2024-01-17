using System;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.customresource.Cards;

namespace TeicsoftSpectacleCards.scripts.customresource;

public static class CardXmlParser {
	public static CardModel ParseCardsFromXml(string filePath) {
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();

		XmlDocument doc = new XmlDocument();
		doc.LoadXml(content);


		XmlNode cardNode = doc.SelectSingleNode("card");

		string cardId = cardNode.Attributes["id"].Value;
		string cardType = cardNode.Attributes["type"].Value;
		string modifier = cardNode.Attributes["modifier"].Value;
		string position = cardNode.Attributes["position"].Value;


		if (Enum.TryParse(modifier, out CardModel.ModifierEnum parsedModifier)) { } else {
			GD.Print("Failed to parse modifier: " + modifier);
		}
		
		if (Enum.TryParse(position, out CardModel.PositionEnum parsedPosition)) { } else {
			GD.Print("Failed to parse position: " + position);
		}


		XmlNode statsNode = cardNode.SelectSingleNode("stats");
		int attack = ParseIntNode(statsNode, "attack");
		int defenseLower = ParseIntNode(statsNode, "defense_lower");
		int defenseUpper = ParseIntNode(statsNode, "defense_upper");
		int health = ParseIntNode(statsNode, "health");
		int draw = ParseIntNode(statsNode, "draw");
		int discard = ParseIntNode(statsNode, "discard");
		int spectaclePoints = ParseIntNode(statsNode, "spectacle_points");

		XmlNode designNode = cardNode.SelectSingleNode("design");
		string imagePath = ParseTextNode(designNode, "image");
		string animationPath = ParseTextNode(designNode, "animation");
		string soundPath = ParseTextNode(designNode, "sound");

		XmlNode textNode = cardNode.SelectSingleNode("text");
		string name = ParseTextNode(textNode, "name");
		string description = ParseTextNode(textNode, "description");
		string lore = ParseTextNode(textNode, "lore");
		string tooltip = ParseTextNode(textNode, "tooltip_text");

		CardModel card = CardFactory.MakeCard(cardType, cardId, parsedModifier, parsedPosition, attack, defenseLower, defenseUpper,
			health, draw, discard, spectaclePoints, name, description, lore, tooltip, imagePath, animationPath, soundPath);

		return card;

		int ParseIntNode(XmlNode parent, string s) {
			return parent.SelectSingleNode(s) != null ? Int32.Parse(parent.SelectSingleNode(s).InnerText) : 0;
		}

		string ParseTextNode(XmlNode parent, string s) {
			return parent.SelectSingleNode(s) != null ? parent.SelectSingleNode(s).InnerText : "";
		}
	}
}
