using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public class ScotchMist : Combo {
    public override void Play(GameState gameState) {
        foreach (Enemy enemy in gameState.Enemies) {
            GD.Print(" **** " + "Hit " + enemy.Name + " for " + Attack);
            enemy.Damage(Attack, Position);
        }
        gameState.SpectacleBuffer += SpectaclePoints;
        GD.Print(" **** " + "Adding " + SpectaclePoints + " SP to buffer");
    }
}
