using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class JudoThrow : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (target.Modifier == Utils.ModifierEnum.Grappled) {
            target.Damage(Attack * 3);
            GD.Print(" * " + "Hit " + target.Name + " for " + Attack * 3);
            if (player is not Enemy) {
                GD.Print(" * " + "Adding " + SpectaclePoints * 3 + " SP to buffer");
                gameState.SpectacleBuffer += SpectaclePoints * 3;
            }
        } else { base.Play(gameState, target, player); }
    }
}
