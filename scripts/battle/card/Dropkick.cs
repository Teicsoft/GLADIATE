using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class Dropkick : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.Modifier = Utils.ModifierEnum.Grounded;
        base.Play(gameState, target, player);
        GD.Print(" * " + "Grounding Self");
    }
}
