using System;
using System.Collections.Generic;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.battle.card;
using FileAccess = Godot.FileAccess;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public class DeckXmlParser {
    public static (string, List<string>) ParseDeckFromXml(string filePath) {
        using FileAccess file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();

        XmlDocument doc = new();
        doc.LoadXml(content);
        XmlNode deckNode = doc.SelectSingleNode("deck");

        string deckId = deckNode.Attributes["id"].Value;
        string deckName = deckNode.Attributes["name"].Value;

        List<string> cards = new();
        foreach (XmlNode cardNode in deckNode.SelectNodes("cards/card")) {
            string cardId = cardNode.Attributes["card_id"].Value;
            cards.Add(cardId);
        }

        return (deckId, cards);
    }

    public static Dictionary<string, List<string>> ParseAllDecks() {
        string deckFilePath = "res://data/decks/";

        string[] filesAtPath = DirAccess.GetFilesAt(deckFilePath);


        Dictionary<string, List<string>> decks = new();
        foreach (string fileName in filesAtPath) {
            if (fileName.EndsWith(".xml") && fileName != "deck_template.xml") {
                (string deckId, List<string> deck) = ParseDeckFromXml(deckFilePath + fileName);
                decks.Add(deckId, deck);
            }
        }

        return decks;
    }
}
