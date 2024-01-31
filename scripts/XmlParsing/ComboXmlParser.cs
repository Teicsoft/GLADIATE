using System;
using System.Collections.Generic;
using System.Xml;
using Godot;
using TeicsoftSpectacleCards.scripts.battle;
using TeicsoftSpectacleCards.scripts.battle.card;

namespace TeicsoftSpectacleCards.scripts.XmlParsing;

public static class ComboXmlParser {

    private static Dictionary<string, Func<Combo>> TypeDictionary = new() {
        { "combo_mgk", () => new DoubleMultCombo() },
        { "combo_EtTuBrutal", () => new SelfDamageCombo() },
        { "combo_AndStayThere", () => new StayJuggledCombo() },
        { "combo_GetPolitical", () => new GetPolitical() },
        { "combo_Footwork", () => new Footwork() },
        { "combo_InciteRiot", () => new InciteRiot() },
        { "combo_ScotchMist", () => new ScotchMist() },
        { "combo_Overwhelm", () => new StunCombo() },
        { "combo_SwearVengeance", () => new SwearVengeance() },
        { "combo_TheAntiThinker", () => new AntiThunker() },
        { "combo_ThroughHardshipTheseScars", () => new ThroughHardshipTheseScars() },
        { "combo_GratuitousBackflip", () => new GratuitousBackflip() },
    };

    public static Combo ParseComboFromXml(string filepath) {
        using FileAccess file = FileAccess.Open(filepath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();

        XmlDocument doc = new();
        doc.LoadXml(content);

        XmlNode comboNode = doc.SelectSingleNode("combo");
        string comboId = comboNode.Attributes["id"].Value;
        string modifier = comboNode.Attributes["modifier"].Value;
        string position = comboNode.Attributes["position"].Value;


        if (!Enum.TryParse(modifier, out battle.Utils.ModifierEnum parsedModifier)) {
            GD.Print("Failed to parse modifier: " + modifier);
        }

        if (!Enum.TryParse(position, out battle.Utils.PositionEnum parsedPosition)) {
            GD.Print("Failed to parse position: " + position);
        }

        List<Card> cardList = new();
        foreach (XmlNode cardNode in comboNode.SelectNodes("cards/card")) {
            string cardId = cardNode.Attributes["card_id"].Value;
            cardList.Add(CardFactory.MakeBlankCard(cardId));
        }

        XmlNode statsNode = comboNode.SelectSingleNode("stats");
        int attack = Utils.ParseIntNode(statsNode, "attack");
        int defenseLower = Utils.ParseIntNode(statsNode, "defense_lower");
        int defenseUpper = Utils.ParseIntNode(statsNode, "defense_upper");
        int health = Utils.ParseIntNode(statsNode, "health");
        int cardDraw = Utils.ParseIntNode(statsNode, "draw");
        int discard = Utils.ParseIntNode(statsNode, "discard");
        int spectaclePoints = Utils.ParseIntNode(statsNode, "spectacle_points");

        XmlNode designNode = comboNode.SelectSingleNode("design");
        string imagePath = Utils.ParseTextNode(designNode, "image");
        string charAnimationPath = Utils.ParseTextNode(designNode, "char_animation");
        string stageAnimationPath = Utils.ParseTextNode(designNode, "stage_animation");
        string soundPath = Utils.ParseTextNode(designNode, "sound");

        XmlNode textNode = comboNode.SelectSingleNode("text");
        string name = Utils.ParseTextNode(textNode, "name");
        string description = Utils.ParseTextNode(textNode, "description");
        string lore = Utils.ParseTextNode(textNode, "lore");
        string onscreenText = Utils.ParseTextNode(textNode, "onscreen_text");

        Combo combo = ConstructCombo(comboId);
        combo.Initialize(comboId, cardList, parsedModifier, parsedPosition, attack, defenseLower, defenseUpper, health,
            cardDraw, discard, spectaclePoints, name, description, lore, onscreenText, imagePath, charAnimationPath,
            stageAnimationPath, soundPath);

        return combo;
    }

    public static List<Combo> ParseAllCombos() {
        string comboFilePath = "res://data/combos/";

        // using DirAccess dir = DirAccess.Open("res://data/combos/");
        string[] filesAtPath = DirAccess.GetFilesAt(comboFilePath);

        List<Combo> allCombos = new List<Combo>();
        foreach (string fileName in filesAtPath) {
            if (!fileName.EndsWith(".xml") || (fileName == "combo_template.xml")) continue;
            allCombos.Add(ParseComboFromXml(comboFilePath + fileName));
        }

        return allCombos;
    }

    private static Combo ConstructCombo(string comboId) {
        return TypeDictionary.GetValueOrDefault(comboId, () => new Combo()).Invoke();
    }
}
