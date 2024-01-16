using System;
using System.Collections.Generic;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.customresource.Cards;
using TeicsoftSpectacleCards.scripts.customresource.deck;
using FileAccess = Godot.FileAccess;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public class DeckXmlParser
{
	public static DeckModel ParseDeckFromXml(string filePath)
	{
		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(content);
		XmlNode deckNode = doc.SelectSingleNode("deck");
		
		string deckId = deckNode.Attributes["id"].Value;
		string deckName = deckNode.Attributes["name"].Value;
		string usedBy = deckNode.Attributes["used_by"].Value;
		
		if (Enum.TryParse(usedBy, out DeckModel.UsedByEnum parsedUsedBy))
		{}
		else
		{
			GD.Print("Failed to parse usedBy: " + usedBy);
		}
		
		XmlNodeList cardNodes = deckNode.SelectNodes("cards/card");
		
		List<CardModel> cardList = new List<CardModel>();

		// int i = 0;
		foreach (XmlNode cardNode in cardNodes)
		{
			string cardId = cardNode.Attributes["card_id"].Value;
			CardModel card = new CardModel(cardId);
			
			cardList.Add(card);
		}
		
		DeckModel deck = new DeckModel(deckId, deckName, parsedUsedBy, cardList);
		
		return deck;
	}
}
