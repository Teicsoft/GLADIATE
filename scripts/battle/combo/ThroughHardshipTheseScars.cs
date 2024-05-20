using Godot;

namespace GLADIATE.scripts.battle;

public class ThroughHardshipTheseScars : Combo {
    public override void Play(GameState gameState) {
        gameState.SpectacleBuffer += gameState.TurnDamageCount;
        gameState.Player.Statuses.Add(Utils.StatusEnum.GetScars);
        base.Play(gameState);
        GD.Print(" **** " + "Adding " + gameState.TurnDamageCount + " SP to buffer (Damage Taken since last turn)");
        GD.Print(" **** " + "Adding 1 of each Block at end of turn");
    }
}
