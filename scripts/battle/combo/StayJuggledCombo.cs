using Godot;

namespace GLADIATE.scripts.battle;

public class StayJuggledCombo : Combo {
    public override void Play(GameState gameState) {
        gameState.GetSelectedEnemy().Statuses.Add(Utils.StatusEnum.StayJuggled);
        base.Play(gameState);
        GD.Print(" **** " + gameState.GetSelectedEnemy().Name + " will not unjuggle during their next turn");
    }
}
