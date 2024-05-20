using System;
using System.Collections.Generic;
using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class FlyingCrossBody : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player is not Enemy) {
            List<Enemy> aliveEnemies = gameState.GetAliveEnemies();
            foreach (Enemy enemy in aliveEnemies) {
                int splitDamage = (int)Math.Floor((double)Attack / aliveEnemies.Count);
                enemy.Damage(splitDamage, TargetPosition);
            }
            gameState.SpectacleBuffer += SpectaclePoints * aliveEnemies.Count;
        } else { base.Play(gameState, target, player); }
        GD.Print(" * " + "Damaging Self for 6");
    }
}
