namespace TeicsoftSpectacleCards.scripts.battle;

public class StayJuggledCombo :Combo{
    public override void Play(GameState gameState) {
        gameState.GetSelectedEnemy().Statuses.Add(Utils.StatusEnum.StayJuggled);
        base.Play(gameState);
    }
}
