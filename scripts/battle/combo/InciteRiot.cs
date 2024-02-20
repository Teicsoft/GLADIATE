using Godot;

namespace GLADIATE.scripts.battle;

public class InciteRiot : Combo {
    public override void Play(GameState gameState) {
        gameState.Enemies[(int)(GD.Randi() % gameState.Enemies.Count)].DirectDamage(10);
        base.Play(gameState);
    }
}
