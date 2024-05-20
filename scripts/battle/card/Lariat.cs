using GLADIATE.scripts.battle.target;
using Godot;

namespace GLADIATE.scripts.battle.card;

public class Lariat : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (target.DefenseUpper > 0) {
            GD.Print(" * " + player.Name + " played " + CardName);
            GD.Print(" * ");
            GD.Print(" * " + "Blocked by " + target.Name + ", who now does double damage!");
            target.Statuses.Add(Utils.StatusEnum.DoubleDamage);
        } else { base.Play(gameState, target, player); }
    }
}
