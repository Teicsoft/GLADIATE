using GLADIATE.scripts.battle.card;
using Godot;

namespace GLADIATE.scenes.glossary;

public partial class CardGlossary : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        Node VBoxContainer = GetNode("ScrollContainer/VBoxContainer");
        
        foreach (string cardId in CardPrototypes.cardPrototypeDict.Keys)
        {
            Card card = CardPrototypes.cardPrototypeDict[cardId];
            
            Node packedScene = ResourceLoader.Load<PackedScene>("res://scenes/glossary/card_glossary_item.tscn").Instantiate();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/CardNameMargin/CardName").Text = card.CardName;
            packedScene.GetNode<TextureRect>("VBoxContainer/ImageMargin/CardImage").Texture = (Texture2D)GD.Load(card.ImagePath);
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer/VBoxContainer/Attack").Text = card.Attack.ToString();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer2/VBoxContainer/DefL").Text = card.DefenseLower.ToString();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer3/VBoxContainer/DefH").Text = card.DefenseUpper.ToString();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer4/VBoxContainer/Heal").Text = card.Health.ToString();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer5/VBoxContainer/Draw").Text = card.CardDraw.ToString();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer6/VBoxContainer/Discard").Text = card.Discard.ToString();
            VBoxContainer.AddChild(packedScene);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
