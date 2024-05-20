using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class ShowOff : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        base.Play(gameState, target, player);
        if (player is not Enemy) {
            GD.Print(" * " + "Next Combo (after the current stack clears) has double SP");
            player.Statuses.Add(Utils.StatusEnum.JustShowedOff);
        }
    }
}
