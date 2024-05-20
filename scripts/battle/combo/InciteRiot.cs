using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public class InciteRiot : Combo {
    public override void Play(GameState gameState) {
        Enemy randomEnemy = gameState.Enemies[(int)(GD.Randi() % gameState.Enemies.Count)];
        randomEnemy.DirectDamage(10);
        base.Play(gameState);
        GD.Print(" **** " + "Directly Damaging " + randomEnemy.Name);
    }
}
