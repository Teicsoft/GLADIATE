using GLADIATE.scripts.battle.target;

namespace GLADIATE.scripts.battle.card;

public class Lariat : Card {
    public override void Play(GameState gameState, ITarget target, ITarget player) {
        if (target.DefenseUpper > 0) { target.Statuses.Add(Utils.StatusEnum.DoubleDamage); } else {
            base.Play(gameState, target, player);
        }
    }
}
