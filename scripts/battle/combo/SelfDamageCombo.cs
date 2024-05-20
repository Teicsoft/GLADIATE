using Godot;

namespace GLADIATE.scripts.battle;

public class SelfDamageCombo : Combo {
    public override void Play(GameState gameState) {
        gameState.Player.DirectDamage(5);
        base.Play(gameState);
        GD.Print(" **** " + "Damaging self for 5");
    }
}
