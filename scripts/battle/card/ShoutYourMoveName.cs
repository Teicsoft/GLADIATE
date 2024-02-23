using System;
using GLADIATE.scripts.battle.target;
using Godot;
using Godot.Collections;
using Array = System.Array;

namespace GLADIATE.scripts.battle.card;

public class ShoutYourMoveName : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        Array values = Enum.GetValues(typeof(Utils.PositionEnum));
        Utils.PositionEnum targetPosition = (Utils.PositionEnum)values.GetValue(GD.Randi() % values.Length);
        if (player is not Enemy) {
            gameState.Enemies.ForEach(e => e.ModifyBlock(1, targetPosition));
            player.Statuses.Add(Utils.StatusEnum.MoveShouted);
        } else { target.ModifyBlock(1, targetPosition); }
    }
}
