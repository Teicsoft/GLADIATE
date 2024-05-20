using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class Spartackle : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        GD.Print(" * " + player.Name + " played " + CardName);
        GD.Print(" * ");
        int modifiedAttack = target.Modifier.Equals(Utils.ModifierEnum.Grounded) ? Attack * 2 : Attack;
        GD.Print(" * " + "Hit " + target.Name + " for " + modifiedAttack);
        target.Damage(modifiedAttack, TargetPosition);
        if (player is not Enemy) {
            GD.Print(" * " + "Adding " + SpectaclePoints + " SP to buffer");
            gameState.SpectacleBuffer += SpectaclePoints;
            gameState.ComboCheck(this);
        }
    }
}
