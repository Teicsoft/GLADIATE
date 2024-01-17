using System;
using System.Collections.Generic;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.customresource.Cards;
using TeicsoftSpectacleCards.scripts.customresource.combos;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public static class ComboXmlParser
{
	public static ComboModel ParseComboFromXml(string filepath)
	{
		using var file = FileAccess.Open(filepath, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(content);
		
		XmlNode comboNode = doc.SelectSingleNode("combo");
		string comboId = comboNode.Attributes["id"].Value;
		string modifier = comboNode.Attributes["modifier"].Value;
		string position = comboNode.Attributes["position"].Value;

		
		if (Enum.TryParse(modifier, out ComboModel.ModifierEnum parsedModifier))
		{}
		else
		{
			GD.Print("Failed to parse modifier: " + modifier);
		}
		
		if (Enum.TryParse(position, out ComboModel.PositionEnum parsedPosition)) { } else {
			GD.Print("Failed to parse position: " + position);
		}
		
		
		XmlNodeList cardNodes = comboNode.SelectNodes("cards/card");
		List<CardModel> CardList = new List<CardModel>();

		int i = 0;
		foreach (XmlNode cardNode in cardNodes)
		{
			string cardId = cardNode.Attributes["card_id"].Value;
			CardModel card = new CardModel(cardId);
			
			CardList.Add(card);
			i++;
		}
		
		XmlNode statsNode = comboNode.SelectSingleNode("stats");
		int attack = statsNode.SelectSingleNode("attack") != null ? Int32.Parse(statsNode.SelectSingleNode("attack").InnerText) : 0;
		int defenseLower = statsNode.SelectSingleNode("defense_lower") != null ? Int32.Parse(statsNode.SelectSingleNode("defense_lower").InnerText) : 0;
		int defenseUpper = statsNode.SelectSingleNode("defense_upper") != null ? Int32.Parse(statsNode.SelectSingleNode("defense_upper").InnerText) : 0;
		int health = statsNode.SelectSingleNode("health") != null ? Int32.Parse(statsNode.SelectSingleNode("health").InnerText) : 0;
		int draw = statsNode.SelectSingleNode("draw") != null ? Int32.Parse(statsNode.SelectSingleNode("draw").InnerText) : 0;
		int discard = ParseIntNode(statsNode, "discard");
		int spectaclePoints = statsNode.SelectSingleNode("spectacle_points") != null ? Int32.Parse(statsNode.SelectSingleNode("spectacle_points").InnerText) : 0;

		XmlNode designNode = comboNode.SelectSingleNode("design");
		string imagePath = designNode.SelectSingleNode("image") != null ? designNode.SelectSingleNode("image").InnerText : "";
		string charAnimationPath = designNode.SelectSingleNode("char_animation") != null ? designNode.SelectSingleNode("char_animation").InnerText : "";
		string stageAnimationPath = designNode.SelectSingleNode("stage_animation") != null ? designNode.SelectSingleNode("stage_animation").InnerText : "";
		string soundPath = designNode.SelectSingleNode("sound") != null ? designNode.SelectSingleNode("sound").InnerText : "";

		XmlNode textNode = comboNode.SelectSingleNode("text");
		string name = textNode.SelectSingleNode("name") != null ? textNode.SelectSingleNode("name").InnerText : "";
		string description = textNode.SelectSingleNode("description") != null ? textNode.SelectSingleNode("description").InnerText : "";
		string lore = textNode.SelectSingleNode("lore") != null ? textNode.SelectSingleNode("lore").InnerText : "";
		string onscreenText = textNode.SelectSingleNode("onscreen_text") != null ? textNode.SelectSingleNode("onscreen_text").InnerText : "";
		
		ComboModel combo = new ComboModel(
			comboId, CardList, parsedModifier, parsedPosition,
			attack, defenseLower, defenseUpper, health, draw, discard, spectaclePoints,
			name, description, lore, onscreenText,
			imagePath, charAnimationPath, stageAnimationPath, soundPath
			);

		return combo;
		
		int ParseIntNode(XmlNode parent, string s) {
			return parent.SelectSingleNode(s) != null ? Int32.Parse(parent.SelectSingleNode(s).InnerText) : 0;
		}

		string ParseTextNode(XmlNode parent, string s) {
			return parent.SelectSingleNode(s) != null ? parent.SelectSingleNode(s).InnerText : "";
		}
	}
}
