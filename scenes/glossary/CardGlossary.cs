using GLADIATE.scripts.battle.card;
using Godot;

namespace GLADIATE.scenes.glossary;

public partial class CardGlossary : Control
{
    public override void _Ready()
    {

        Node VBoxContainer = GetNode("ScrollContainer/VBoxContainer");
        
        foreach (string cardId in CardFactory.CardPrototypeDict.Keys)
        {
            Card card = CardFactory.CardPrototypeDict[cardId];
            
            Node packedScene = ResourceLoader.Load<PackedScene>("res://scenes/glossary/card_glossary_item.tscn").Instantiate();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/CardNameMargin/CardName").Text = card.CardName;
            packedScene.GetNode<TextureRect>("VBoxContainer/ImageMargin/CardImage").Texture = (Texture2D)GD.Load(card.ImagePath);
            
            // Stats
            string statsPathPrefix = "VBoxContainer/ContentMargin/VBoxContainer/StatsGridItemMargin/HBoxContainer/MarginContainer";
            packedScene.GetNode<Label>(statsPathPrefix + "1/VBoxContainer/Attack").Text = card.Attack.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "2/VBoxContainer/DefL").Text = card.DefenseLower.ToString();
            packedScene.GetNode<Label>(statsPathPrefix+ "3/VBoxContainer/DefH").Text = card.DefenseUpper.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "4/VBoxContainer/Heal").Text = card.Health.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "5/VBoxContainer/Draw").Text = card.CardDraw.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "6/VBoxContainer/Discard").Text = card.Discard.ToString();
            packedScene.GetNode<Label>(statsPathPrefix + "7/VBoxContainer/Spectacle").Text = card.SpectaclePoints.ToString();

            //text
            string textPathPrefix = "VBoxContainer/ContentMargin/VBoxContainer/DescriptionGridItemMargin2/VBoxContainer/MarginContainer";
            packedScene.GetNode<Label>(textPathPrefix + "/VBoxContainer/Description").Text = card.Description;
            packedScene.GetNode<Label>(textPathPrefix + "2/VBoxContainer/Lore").Text = card.Lore;

            //pos/mod/type/target
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/PosModMargin/VBoxContainer/POSMODHBoxContainer/MarginContainer/PosHBoxContainer/Position").Text = card.TargetPosition.ToString();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/PosModMargin/VBoxContainer/POSMODHBoxContainer/MarginContainer2/ModHBoxContainer2/Modifier").Text = card.Modifier.ToString();
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/PosModMargin/VBoxContainer/TRTHBoxContainer2/MarginContainer4/ModHBoxContainer2/Type").Text = card.CardType;
            packedScene.GetNode<Label>("VBoxContainer/ContentMargin/VBoxContainer/PosModMargin/VBoxContainer/TRTHBoxContainer2/MarginContainer3/ModHBoxContainer2/TargetRequired").Text = card.TargetRequired.ToString();
            
            
            VBoxContainer.AddChild(packedScene);
        }
    }
    
    private void OnCardGlossaryPressed()
    {
        GetTree().Paused = true;
        Show();
    }
    private void OnCloseCardGlossarySelected()
    {
        Hide();
        GetTree().Paused = false;
    }
}
