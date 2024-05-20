using Godot;

namespace GLADIATE.scripts.battle;

public class StunCombo : Combo {
    public override void Play(GameState gameState) {
        gameState.GetSelectedEnemy().Stun();
        base.Play(gameState);
        GD.Print(" **** " + "Stunning" + gameState.GetSelectedEnemy().Name);
    }
}
