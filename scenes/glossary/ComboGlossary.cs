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
    
    public override void _Ready()
    {
        Node VBoxContainer = GetNode<Node>("ScrollContainer/VBoxContainer");
        
        AllCombos = ComboXmlParser.ParseAllCombos();
        foreach (Combo combo in AllCombos)
        {
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
                
                cardList.AddChild(label);
                i++;
            }
            
            VBoxContainer.AddChild(packedScene);
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
