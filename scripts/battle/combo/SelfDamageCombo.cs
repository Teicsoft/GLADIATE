using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle;

public class SelfDamageCombo : Combo {
    public override void Play(GameState gameState) {
        gameState.Player.DirectDamage(5);
        base.Play(gameState);
    }
}
