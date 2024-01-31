using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class JudoThrow : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (target.Modifier == Utils.ModifierEnum.Grappled) {
            target.Damage(Attack * 3);
            if (player is not Enemy) { gameState.SpectacleBuffer += SpectaclePoints * 3; }
        } else { base.Play(gameState, target, player); }
    }
}
