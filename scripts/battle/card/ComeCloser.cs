using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class ComeCloser : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.Statuses.Add(Utils.StatusEnum.Countering);
        base.Play(gameState, target, player);
        GD.Print(" * " + "Ready to Counter");
    }
}
