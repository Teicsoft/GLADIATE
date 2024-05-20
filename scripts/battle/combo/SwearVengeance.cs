using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public class SwearVengeance : Combo {
    public override void Play(GameState gameState) {
        base.Play(gameState);
        Enemy selectedEnemy = gameState.GetSelectedEnemy();
        if (selectedEnemy.Health <= selectedEnemy.MaxHealth / 5) {
            GD.Print(" **** " + "Triggering Insta-kill");
            selectedEnemy.Health = 0;
        }
        if (gameState.Player.Health <= gameState.Player.MaxHealth / 5) {
            GD.Print(" **** " + "Adding 50 SP to buffer");
            gameState.SpectacleBuffer += 50;
        }
    }
}
