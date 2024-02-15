namespace GLADIATE.scripts.battle;

public class DoubleMultCombo : Combo {
    public override void Play(GameState gameState) {
        gameState.Multiplier *= 2;
        base.Play(gameState);
    }
}
