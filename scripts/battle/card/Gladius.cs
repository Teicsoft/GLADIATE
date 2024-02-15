using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class Gladius : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (target.Modifier == Utils.ModifierEnum.Juggled) {
            // Added once here, added again during base.Play()
            gameState.SpectacleBuffer += SpectaclePoints;
        }
        base.Play(gameState, target, player);
    }
}
