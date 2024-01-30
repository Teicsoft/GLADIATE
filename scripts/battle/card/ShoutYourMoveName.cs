﻿using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class ShoutYourMoveName : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player is not Enemy) {
            gameState.Enemies.ForEach(e => e.ModifyBlock(1, TargetPosition));
            gameState.ComboCheck(this);
        } else { target.ModifyBlock(1, TargetPosition); }
    }
}