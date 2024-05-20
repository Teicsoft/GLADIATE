using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class BloodOnSand : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.DirectDamage(6);
        base.Play(gameState, target, player);
        GD.Print(" * " + "Damaging Self for 6");
    }
}
