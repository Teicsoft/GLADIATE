namespace GLADIATE.scripts.battle;

public class GratuitousBackflip : Combo {
    public override void Play(GameState gameState) {
        if (gameState.Player.Statuses.Contains(Utils.StatusEnum.OpenedRecklessly)) {
            gameState.Player.Stun();
        }
        base.Play(gameState);
    }
}
