using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class Dropkick : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.Modifier = Utils.ModifierEnum.Grounded;
        base.Play(gameState, target, player);
    }
}
