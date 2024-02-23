using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle;

public class GratuitousBackflip : Combo {
    public override void Play(GameState gameState) {
        Player player = gameState.Player;
        if (player.Statuses.Contains(Utils.StatusEnum.OpenedRecklessly)) {
            player.Stun();
            if (player.IsStunned()) {
                gameState.EndTurn();
            }
        }
        base.Play(gameState);
    }
}
