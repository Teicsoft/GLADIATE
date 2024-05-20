using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public class GratuitousBackflip : Combo {
    public override void Play(GameState gameState) {
        base.Play(gameState);
        Player player = gameState.Player;
        if (player.Statuses.Contains(Utils.StatusEnum.OpenedRecklessly)) {
            player.Stun();
            if (player.IsStunned()) {
                GD.Print(" **** " + "Reckless Open blocked, user losing turn");
                gameState.EndTurn();
            }
        }
    }
}
