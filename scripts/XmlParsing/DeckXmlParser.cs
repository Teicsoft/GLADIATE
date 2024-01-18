using System;
using System.Collections.Generic;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using TeicsoftSpectacleCards.scripts.XmlParsing.models;
using FileAccess = Godot.FileAccess;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public class DeckXmlParser {
    public static DeckModel ParseDeckFromXml(string filePath) {
        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();

        XmlDocument doc = new();
        doc.LoadXml(content);
        XmlNode deckNode = doc.SelectSingleNode("deck");

        string deckId = deckNode.Attributes["id"].Value;
        string deckName = deckNode.Attributes["name"].Value;
        string usedBy = deckNode.Attributes["used_by"].Value;

        if (!Enum.TryParse(usedBy, out DeckModel.UsedByEnum parsedUsedBy)) {
            GD.Print("Failed to parse usedBy: " + usedBy);
        }

        List<Card> cardList = new();
        foreach (XmlNode cardNode in deckNode.SelectNodes("cards/card")) {
            string cardId = cardNode.Attributes["card_id"].Value;
            cardList.Add(CardFactory.MakeBlankCard(cardId));
        }

        return new(deckId, deckName, parsedUsedBy, cardList);
    }
}
