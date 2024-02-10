using System;
using System.Collections.Generic;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.XmlParsing;

namespace TeicsoftSpectacleCards.scripts.battle.card;

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


        if (!Enum.TryParse(modifier, out battle.Utils.ModifierEnum parsedModifier)) {
            GD.Print("Failed to parse modifier: " + modifier);
        }

        if (!Enum.TryParse(position, out battle.Utils.PositionEnum parsedPosition)) {
            GD.Print("Failed to parse position: " + position);
        }


        XmlNode statsNode = cardNode.SelectSingleNode("stats");
        int attack = XmlParsing.Utils.ParseIntNode(statsNode, "attack");
        int defenseLower = XmlParsing.Utils.ParseIntNode(statsNode, "defense_lower");
        int defenseUpper = XmlParsing.Utils.ParseIntNode(statsNode, "defense_upper");
        int health = XmlParsing.Utils.ParseIntNode(statsNode, "heal");
        int draw = XmlParsing.Utils.ParseIntNode(statsNode, "draw");
        int discard = XmlParsing.Utils.ParseIntNode(statsNode, "discard");
        int spectaclePoints = XmlParsing.Utils.ParseIntNode(statsNode, "spectacle_points");
        bool targetRequired = XmlParsing.Utils.ParseBoolNode(statsNode, "target_required");

        XmlNode designNode = cardNode.SelectSingleNode("design");
        string imagePath = XmlParsing.Utils.ParseTextNode(designNode, "image");
        string animationPath = XmlParsing.Utils.ParseTextNode(designNode, "animation");
        string soundPath = XmlParsing.Utils.ParseTextNode(designNode, "sound");

        XmlNode textNode = cardNode.SelectSingleNode("text");
        string name = XmlParsing.Utils.ParseTextNode(textNode, "name");
        string description = XmlParsing.Utils.ParseTextNode(textNode, "description");
        string lore = XmlParsing.Utils.ParseTextNode(textNode, "lore");
        string tooltip = XmlParsing.Utils.ParseTextNode(textNode, "tooltip_text");

        return CardFactory.MakeCard(
            cardType, cardId, parsedModifier, parsedPosition, attack, defenseLower, defenseUpper, health, draw, discard,
            spectaclePoints, name, description, lore, tooltip, imagePath, animationPath, soundPath, targetRequired
        );
    }

    public static Dictionary<string, Card> ParseAllCards() {
        string cardFilePath = "res://data/cards/";

        string[] filesAtPath = DirAccess.GetFilesAt(cardFilePath);


        Dictionary<string, Card> cardList = new Dictionary<string, Card>();
        foreach (string fileName in filesAtPath) {
            if (fileName.EndsWith(".xml") && fileName != "card_template.xml") {
                Card card = ParseCardsFromXml(cardFilePath + fileName);
                cardList.Add(card.Id, card);
            }
        }

        return cardList;
    }
}

public static class CardPrototypes {
    public static Dictionary<string, Card> cardPrototypeDict = CardXmlParser.ParseAllCards();

    public static Card CloneCard(string cardId) { return cardPrototypeDict[cardId].Clone(); }

    public static void ReloadCardPrototypes() { cardPrototypeDict = CardXmlParser.ParseAllCards(); }
}

public static class CardFactory {
    private static Dictionary<string, Func<Card>> TypeDictionary = new() {
        { "card_reckless", () => new StunCard() },
        { "card_Spartackle", () => new Spartackle() },
        { "card_BloodOnTheSand", () => new SelfDamageCard() },
        { "card_gladius", () => new Gladius() },
        { "card_Grapple", () => new ModifierCard() },
        { "card_FlyingCrossBody", () => new FlyingCrossBody() },
        { "card_DropKick", () => new Dropkick() },
        { "card_headbutt", () => new SelfDamageCard() },
        { "card_JudoThrow", () => new JudoThrow() },
        { "card_KipUp", () => new KipUp() },
        { "card_Throw", () => new ModifierCard() },
        { "card_Trip", () => new ModifierCard() },
        { "card_Showoff", () => new ShowOff() },
        { "card_PleaseTheCrowd", () => new PleaseTheCrowd() },
        { "card_DramaticTattooReveal", () => new DramaticTattooReveal() },
        { "card_GallicSuplex", () => new GallicSuplex() },
        { "card_ComeCloser", () => new ComeCloser() },
        { "card_ShoutYourMoveName", () => new ShoutYourMoveName() },
        { "card_Lariat", () => new Lariat() },
    };

    public static Card ConstructCard(string cardId) {
        return TypeDictionary.GetValueOrDefault(cardId, () => new Card()).Invoke();
    }

    public static Card MakeBlankCard(string cardId) {
        Card card = ConstructCard(cardId);
        card.Id = cardId;
        return card;
    }

    public static Card MakeCard(
        string type, string cardId, Utils.ModifierEnum modifier, Utils.PositionEnum position, int attack,
        int defenseLower, int defenseUpper, int health, int draw, int discard, int spectaclePoints, string name,
        string description, string lore, string tooltip, string imagePath, string animationPath, string soundPath,
        bool targetRequired
    ) {
        Card card = ConstructCard(cardId);

        return card.Initialize(
            type, cardId, modifier, position, targetRequired, attack, defenseLower, defenseUpper, health, draw, discard,
            spectaclePoints, name, description, lore, tooltip, imagePath, animationPath, soundPath
        );
    }
}
