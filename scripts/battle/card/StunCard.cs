using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class StunCard : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        target.Stun();
        base.Play(gameState, target, player);
        GD.Print(" * " + "Stunning Target");
    }
}
