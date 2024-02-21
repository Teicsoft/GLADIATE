using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public class ShoutYourMoveName : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (player is not Enemy) {
            gameState.Enemies.ForEach(e => e.ModifyBlock(1, TargetPosition));
            player.Statuses.Add(Utils.StatusEnum.MoveShouted);
        } else { target.ModifyBlock(1, TargetPosition); }
    }
}
