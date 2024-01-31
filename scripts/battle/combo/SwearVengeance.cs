using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle;

public class SwearVengeance :Combo{
    public override void Play(GameState gameState) {
        base.Play(gameState);
        Enemy selectedEnemy = gameState.GetSelectedEnemy();
        if (selectedEnemy.Health <= selectedEnemy.MaxHealth / 5) {
            selectedEnemy.Health = 0;
        }
        if (gameState.Player.Health <= gameState.Player.MaxHealth / 5) {
            gameState.SpectacleBuffer += 50;
        }
    }
}
