using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class KipUp : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player.Modifier == Utils.ModifierEnum.Grounded) { player.Modifier = Utils.ModifierEnum.None; }
        base.Play(gameState, target, player);
        GD.Print(" * " + "Ungrounding Self");
    }
}
