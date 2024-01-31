using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class DramaticTattooReveal : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player is not Enemy) {
            gameState.Enemies.ForEach(e => e.Statuses.Add(Utils.StatusEnum.TattooRevealed));
        } else { target.Statuses.Add(Utils.StatusEnum.TattooRevealed); }
        base.Play(gameState, target, player);
    }
}
