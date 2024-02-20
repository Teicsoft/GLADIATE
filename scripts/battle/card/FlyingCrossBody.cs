using System;
using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public class FlyingCrossBody : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player is not Enemy) {
            foreach (Enemy enemy in gameState.Enemies) {
                enemy.Damage((int)Math.Floor((double)Attack / gameState.Enemies.Count), TargetPosition);
            }
            gameState.SpectacleBuffer += SpectaclePoints * gameState.Enemies.Count;
        } else { base.Play(gameState, target, player); }
    }
}
