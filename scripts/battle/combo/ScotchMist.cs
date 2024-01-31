using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle;

public class ScotchMist : Combo {
    public override void Play(GameState gameState) {
        foreach (Enemy enemy in gameState.Enemies) { enemy.Damage(Attack, Position); }
        gameState.SpectacleBuffer += SpectaclePoints;
    }
}
