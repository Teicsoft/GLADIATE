using GLADIATE.scripts.battle.card;
using Godot;

namespace GLADIATE.scenes.glossary;

public partial class CardGlossary : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
        foreach (string cardId in CardPrototypes.cardPrototypeDict.Keys)
        {
            GD.Print(cardId);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
