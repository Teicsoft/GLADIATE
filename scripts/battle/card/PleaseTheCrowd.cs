using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class PleaseTheCrowd : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        player.Statuses.Add(Utils.StatusEnum.CrowdPleased);
        base.Play(gameState, target, player);
        GD.Print(" * " + "Draw cards at end of turn for each enemy defeated");
    }
}
