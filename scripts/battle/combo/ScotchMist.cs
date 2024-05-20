using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public class ScotchMist : Combo {
    public override void Play(GameState gameState) {
        foreach (Enemy enemy in gameState.Enemies) {
            Utils.DoAttack(gameState, enemy, gameState.Player, Attack, Position);
            GD.Print(" **** " + "Hit " + enemy.Name + " for " + Utils.CalculateDamage(gameState.Player, Attack));
        }
        gameState.SpectacleBuffer += SpectaclePoints;
        GD.Print(" **** " + "Adding " + SpectaclePoints + " SP to buffer");
    }
}
