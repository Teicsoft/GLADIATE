using Godot;
using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class StunCard : Card {

    public override void Play(GameState gameState, ITarget target, ITarget player) {
        GD.Print("STUN!");
        target.Stun(1);
        base.Play(gameState, target, player);
    }

}
