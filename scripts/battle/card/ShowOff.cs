using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class ShowOff : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player is not Enemy) { player.Statuses.Add(Utils.StatusEnum.JustShowedOff); }
        base.Play(gameState, target, player);
    }
}
