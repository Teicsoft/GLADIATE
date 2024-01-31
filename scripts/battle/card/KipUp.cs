using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class KipUp:Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player.Modifier == Utils.ModifierEnum.Grounded) {
            player.Modifier = Utils.ModifierEnum.None;
        }
        base.Play(gameState, target, player);
    }
}
