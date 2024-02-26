namespace GLADIATE.scripts.battle;

public class ThroughHardshipTheseScars : Combo {
    public override void Play(GameState gameState) {
        gameState.SpectacleBuffer += gameState.TurnDamageCount;
        gameState.Player.Statuses.Add(Utils.StatusEnum.GetScars);
        base.Play(gameState);
    }
}
