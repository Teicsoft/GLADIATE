using System;
using GLADIATE.scripts.battle.target;
using Godot;
using Godot.Collections;
using Array = System.Array;

namespace GLADIATE.scripts.battle.card;

public class ShoutYourMoveName : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        GD.Print(" * " + player.Name + " played " + CardName);
        GD.Print(" * ");
        GD.Print(" * " + "Giving one random Block to every opponent");
        if (player is not Enemy) {
            gameState.Enemies.ForEach(e => e.ModifyBlock(1, Utils.GetRandomPosition()));
            player.Statuses.Add(Utils.StatusEnum.MoveShouted);
            GD.Print(" * " + "+10 SP for other cards this turn");
        } else { target.ModifyBlock(1, Utils.GetRandomPosition()); }
    }
}
