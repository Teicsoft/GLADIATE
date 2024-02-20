using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public class Dropkick : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.Modifier = Utils.ModifierEnum.Grounded;
        base.Play(gameState, target, player);
    }
}
