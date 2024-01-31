using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class PleaseTheCrowd : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.Statuses.Add(Utils.StatusEnum.CrowdPleased);
        base.Play(gameState, target, player);
    }
}
