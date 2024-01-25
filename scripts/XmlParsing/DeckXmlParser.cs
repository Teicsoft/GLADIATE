using System;
using System.Collections.Generic;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using FileAccess = Godot.FileAccess;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public class DeckXmlParser {
    public static Deck ParseDeckFromXml(string filePath) {
        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();

        XmlDocument doc = new();
        doc.LoadXml(content);
        XmlNode deckNode = doc.SelectSingleNode("deck");

        string deckId = deckNode.Attributes["id"].Value;
        string deckName = deckNode.Attributes["name"].Value;
        string usedBy = deckNode.Attributes["used_by"].Value;

        if (!Enum.TryParse(usedBy, out Deck.UsedBy parsedUsedBy)) { GD.Print("Failed to parse usedBy: " + usedBy); }

        List<Card> cardList = new();
        foreach (XmlNode cardNode in deckNode.SelectNodes("cards/card")) {
            string cardId = cardNode.Attributes["card_id"].Value;

            // cardList.Add(CardFactory.MakeBlankCard(cardId));
            cardList.Add(CardPrototypes.CloneCard(cardId));
        }

        Deck deck = new Deck();

        List<CardSleeve> cardSleeves = Deck.SleeveCards(cardList);

        return deck.Initialize(deckId, deckName, parsedUsedBy, cardSleeves);
    }
}
