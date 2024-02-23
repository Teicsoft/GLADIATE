using System;
using GLADIATE.scripts.battle.target;
using Godot;
using Godot.Collections;
using Array = System.Array;

namespace GLADIATE.scripts.battle.card;

public class ShoutYourMoveName : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player is not Enemy) {
            gameState.Enemies.ForEach(
                e => e.ModifyBlock(1, Utils.GetRandomPosition())
            );
            player.Statuses.Add(Utils.StatusEnum.MoveShouted);
        } else { target.ModifyBlock(1, Utils.GetRandomPosition()); }
    }
}
