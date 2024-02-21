using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle;

public class GetPolitical : Combo {
    public override void Play(GameState gameState) {
        foreach (Enemy enemy in gameState.Enemies) {
            enemy.ModifyBlock(-1, GD.Randi() % 2 == 0 ? Utils.PositionEnum.Upper : Utils.PositionEnum.Lower);
        }
        base.Play(gameState);
    }
}
