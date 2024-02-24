using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GLADIATE.scripts.battle;
using GLADIATE.scripts.battle.card;
using GLADIATE.scripts.XmlParsing;

public partial class ComboGlossary : Control
{
    private List<Combo> AllCombos;
    private Node VBoxContainer;
    
    public override void _Ready()
    {
        VBoxContainer = GetNode<Node>("ScrollContainer/VBoxContainer");
    }

    public void Initialize(Deck<CardSleeve> deck)
    {
        AllCombos = ComboXmlParser.ParseAllCombos();
        List<Node> combosNotInDeck = new();
        
        foreach (Combo combo in AllCombos)
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

            foreach (Card card in combo.CardList)
            {
                Label label = new Godot.Label();

                if (_blockCards.Contains(card.Id))
                {
                    label.Text = (i+1).ToString() + ": " + "Any Block Card";
                }
                else
                {
                    label.Text = (i+1).ToString() + ": " + CardPrototypes.cardPrototypeDict[card.Id].CardName;
                }
                
                if ((deck.Cards.Any(c => c.Card.Id == card.Id) == false)  && inDeck)
                {
                    combosNotInDeck.Add(packedScene);
                    inDeck = false;
                    GD.Print("Combo not in deck: " + combo.Name + " due to " + card.Id);
                }
                
                cardList.AddChild(label);
                i++;
            }

            if (inDeck)
            {
                
                GD.Print("Combo in deck: " + combo.Name);
                packedScene.GetNode<TextureRect>("VBoxContainer/ContentMargin/VBoxContainer/ComboNameMargin/ComboName/ThumbsUp").Show();
                VBoxContainer.AddChild(packedScene);
            }
        }

        foreach (Node comboNotInDeck in combosNotInDeck)
        {
            VBoxContainer.AddChild(comboNotInDeck);
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
