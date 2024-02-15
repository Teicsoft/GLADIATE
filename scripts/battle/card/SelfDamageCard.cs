using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public class SelfDamageCard : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.DirectDamage(5);
        base.Play(gameState, target, player);
    }
}
