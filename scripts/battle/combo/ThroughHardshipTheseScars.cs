namespace TeicsoftSpectacleCards.scripts.battle;

public class ThroughHardshipTheseScars : Combo {
    public override void Play(GameState gameState) {
        // TODO: Gain additional SP equal to damage taken this turn.
        gameState.Player.Statuses.Add(Utils.StatusEnum.GetScars);
        base.Play(gameState);
    }
}
