using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public class RecklessOpen :Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.Statuses.Add(Utils.StatusEnum.OpenedRecklessly);
        base.Play(gameState, target, player);
    }
    
}
