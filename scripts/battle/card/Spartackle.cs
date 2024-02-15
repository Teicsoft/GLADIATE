using TeicsoftSpectacleCards.scripts.battle.target;

namespace TeicsoftSpectacleCards.scripts.battle.card;

public class Spartackle : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        target.Damage(target.Modifier.Equals(Utils.ModifierEnum.Grounded) ? Attack * 2 : Attack, TargetPosition);
        if (player is not Enemy) { gameState.ComboCheck(this); }
    }
}
