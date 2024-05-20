using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class JudoThrow : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (target.Modifier == Utils.ModifierEnum.Grappled) {
            Utils.DoAttack(gameState, target, player, Attack * 3, Utils.PositionEnum.Upper);
            GD.Print(" * " + "Hit " + target.Name + " for " + Utils.CalculateDamage(player, Attack * 3));
            if (player is not Enemy) {
                GD.Print(" * " + "Adding " + SpectaclePoints * 3 + " SP to buffer");
                gameState.SpectacleBuffer += SpectaclePoints * 3;
            }
        } else { base.Play(gameState, target, player); }
    }
}
