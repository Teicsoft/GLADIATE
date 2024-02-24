using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.battle;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.XmlParsing;

public partial class ComboGlossary : Control
{
    private Node _vBoxContainer;
    
    public override void _Ready()
    {
        _vBoxContainer = GetNode<Node>("ScrollContainer/VBoxContainer");
    }

    public void Initialize(Deck<CardSleeve> deck, List<Combo> allCombos)
    {
        List<Node> combosNotInDeck = new();
        
        foreach (Combo combo in allCombos)
        {
            bool inDeck = true;
            Node packedScene = ResourceLoader.Load<PackedScene>("res://scenes/glossary/combo_glossary_item.tscn").Instantiate();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/ComboNameMargin/ComboName").Text = combo.Name;
            
            // Stats
            string statsPathPrefix = "VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer";
            packedScene.GetNode<Label>(statsPathPrefix + "1/VBoxContainer/Attack").Text = combo.Attack.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "2/VBoxContainer/DefL").Text = combo.DefenseLower.ToString();
            packedScene.GetNode<Label>(statsPathPrefix+ "3/VBoxContainer/DefH").Text = combo.DefenseUpper.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "4/VBoxContainer/Heal").Text = combo.Health.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "5/VBoxContainer/Draw").Text = combo.CardDraw.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "6/VBoxContainer/Discard").Text = combo.Discard.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "7/VBoxContainer/Spectacle").Text = combo.SpectaclePoints.ToString();
            
            
            //text
            string textPathPrefix = "VBoxContainer/ContentMargin/VBoxContainer/DescriptionGridItemMargin2/VBoxContainer/MarginContainer";
            packedScene.GetNode<Label>(textPathPrefix + "/VBoxContainer/Description").Text = combo.Description;
            packedScene.GetNode<Label>(textPathPrefix + "2/VBoxContainer/Lore").Text = combo.Lore;
            
            //cardList
            VBoxContainer cardList = packedScene.GetNode<VBoxContainer>("VBoxContainer/ContentMargin/VBoxContainer/CardList/VBoxContainer");
            int i = 0;
            List<string> _blockCards = CardPrototypes.cardPrototypeDict.Where(kvp => kvp.Value.CardType == "Block").Select(kvp => kvp.Value.Id).ToList();

            foreach (Card comboCard in combo.CardList)
            {
                Label label = new Godot.Label();

                if (comboCard.Id == "card_FullBlock")
                {
                    label.Text = (i+1).ToString() + ": " + "Any Block Card";
                }
                else
                {
                    label.Text = (i+1).ToString() + ": " + CardPrototypes.cardPrototypeDict[comboCard.Id].CardName;
                }
                
                if ((deck.Cards.Any(deckCard => (deckCard.Card.Id == comboCard.Id) || (_blockCards.Contains(deckCard.Card.Id) && "card_FullBlock" == comboCard.Id) ) == false) && inDeck)
                {
                    combosNotInDeck.Add(packedScene);
                    inDeck = false;
                }
                
                cardList.AddChild(label);
                i++;
            }

            if (inDeck)
            {
                packedScene.GetNode<TextureRect>("VBoxContainer/ContentMargin/VBoxContainer/ComboNameMargin/ComboName/ThumbsUp").Show();
                _vBoxContainer.AddChild(packedScene);
            }
        }

        foreach (Node comboNotInDeck in combosNotInDeck)
        {
            _vBoxContainer.AddChild(comboNotInDeck);
        }
    }
    
    private void OnComboGlossaryButtonPressed()
    {
        GetTree().Paused = true;
        Show();
    }
    
    private void OnCloseComboGlossarySelected()
    {
        Hide();
        GetTree().Paused = false;
    }
}
