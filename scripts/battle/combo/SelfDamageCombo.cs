using Godot;

namespace GLADIATE.scripts.battle;

public class SelfDamageCombo : Combo {
    public override void Play(GameState gameState) {
        int selfDamage = 5;
        gameState.Player.DirectDamage(selfDamage);
        base.Play(gameState);
        GD.Print(" * " + "Damaging Self for " + selfDamage);
    }
}
