using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class SelfDamageCard : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.DirectDamage(5);
        base.Play(gameState, target, player);
    }
}
